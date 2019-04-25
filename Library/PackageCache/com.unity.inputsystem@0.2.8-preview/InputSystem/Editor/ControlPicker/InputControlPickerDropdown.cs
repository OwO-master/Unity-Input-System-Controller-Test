#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine.Experimental.Input.Controls;
using UnityEngine.Experimental.Input.Layouts;
using UnityEngine.Experimental.Input.Utilities;

////TODO: have tooltips on each entry in the picker

////TODO: find better way to present controls when filtering to specific devices

namespace UnityEngine.Experimental.Input.Editor
{
    internal class InputControlPickerDropdown : AdvancedDropdown
    {
        public InputControlPickerDropdown(
            InputControlPickerState state,
            Action<string> onPickCallback,
            InputControlPicker.Mode mode = InputControlPicker.Mode.PickControl)
            : base(state.advancedDropdownState)
        {
            m_Gui = new InputControlPickerGUI(this);

            minimumSize = new Vector2(275, 300);
            maximumSize = new Vector2(0, 300);

            m_OnPickCallback = onPickCallback;
            m_Mode = mode;
        }

        public void SetControlPathsToMatch(string[] controlPathsToMatch)
        {
            m_ControlPathsToMatch = controlPathsToMatch;
            Reload();
        }

        public void SetExpectedControlLayout(string expectedControlLayout)
        {
            m_ExpectedControlLayout = expectedControlLayout;
            m_ExpectedControlType = !string.IsNullOrEmpty(expectedControlLayout)
                ? InputSystem.s_Manager.m_Layouts.GetControlTypeForLayout(new InternedString(expectedControlLayout))
                : null;
            Reload();
        }

        protected override void OnDestroy()
        {
            m_RebindingOperation?.Dispose();
            m_RebindingOperation = null;
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(string.Empty);

            // Usages.
            if (m_Mode != InputControlPicker.Mode.PickDevice)
            {
                var usages = BuildTreeForUsages();
                if (usages.children.Any())
                {
                    root.AddChild(usages);
                    root.AddSeparator();
                }
            }

            // Devices.
            AddItemsForDevices(root);

            return root;
        }

        protected override AdvancedDropdownItem BuildCustomSearch(string searchString,
            IEnumerable<AdvancedDropdownItem> elements)
        {
            if (!isListening)
                return null;

            var root = new AdvancedDropdownItem(!string.IsNullOrEmpty(m_ExpectedControlLayout)
                ? $"Listening for {m_ExpectedControlLayout}..."
                : "Listening for input...");

            if (searchString == "\u0017")
                return root;

            var paths = searchString.Substring(1).Split('\u0017');
            foreach (var element in elements)
            {
                if (element is ControlDropdownItem controlItem && paths.Any(x => controlItem.controlPathWithDevice == x))
                    root.AddChild(element);
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            var path = ((InputControlDropdownItem)item).controlPathWithDevice;
            m_OnPickCallback(path);
        }

        private AdvancedDropdownItem BuildTreeForUsages()
        {
            var usageRoot = new AdvancedDropdownItem("Usages");
            foreach (var usageAndLayouts in EditorInputControlLayoutCache.allUsages)
            {
                if (usageAndLayouts.Item2.Any(LayoutMatchesExpectedControlLayoutFilter))
                {
                    var child = new UsageDropdownItem(usageAndLayouts.Item1);
                    usageRoot.AddChild(child);
                }
            }
            return usageRoot;
        }

        private void AddItemsForDevices(AdvancedDropdownItem parent)
        {
            // Add devices that are marked as generic types of devices directly to the parent.
            // E.g. adds "Gamepad" and then underneath all the more specific types of gamepads.
            foreach (var deviceLayout in EditorInputControlLayoutCache.allLayouts
                     .Where(x => x.isDeviceLayout && x.isGenericTypeOfDevice && !x.hideInUI).OrderBy(a => a.displayName))
                AddDeviceTreeItemRecursive(deviceLayout, parent);

            // We have devices that are based directly on InputDevice but are not marked as generic types
            // of devices (e.g. Vive Lighthouses). We do not want them to clutter the list at the root so we
            // all of them in a group called "Other" at the end of the list.
            AdvancedDropdownItem otherGroup = new AdvancedDropdownItem("Other");
            foreach (var deviceLayout in EditorInputControlLayoutCache.allLayouts
                     .Where(x => x.isDeviceLayout && !x.isGenericTypeOfDevice && x.type.BaseType == typeof(InputDevice) &&
                         !x.hideInUI && !x.baseLayouts.Any()).OrderBy(a => a.displayName))
                AddDeviceTreeItemRecursive(deviceLayout, otherGroup);

            if (otherGroup.children.Any())
                parent.AddChild(otherGroup);
        }

        private void AddDeviceTreeItemRecursive(InputControlLayout layout, AdvancedDropdownItem parent, bool searchable = true)
        {
            // Find all layouts directly based on this one.
            var childLayouts = EditorInputControlLayoutCache.allLayouts
                .Where(x => x.isDeviceLayout && !x.hideInUI && x.baseLayouts.Contains(layout.name)).OrderBy(x => x.displayName);

            // See if the entire tree should be excluded.
            var shouldIncludeDeviceLayout = ShouldIncludeDeviceLayout(layout);
            var shouldIncludeAtLeastOneChildLayout = childLayouts.Any(ShouldIncludeDeviceLayout);

            if (!shouldIncludeDeviceLayout && !shouldIncludeAtLeastOneChildLayout)
                return;

            // Add toplevel item for device.
            var deviceItem = new DeviceDropdownItem(layout);
            parent.AddChild(deviceItem);

            // Add common usage variants.
            if (m_Mode != InputControlPicker.Mode.PickDevice && layout.commonUsages.Count > 0)
            {
                foreach (var usage in layout.commonUsages)
                {
                    var usageItem = new DeviceDropdownItem(layout, usage);
                    AddControlTreeItemsRecursive(layout, usageItem, layout.name, usage, searchable);
                    deviceItem.AddChild(usageItem);
                }
                deviceItem.AddSeparator();
            }

            // Add controls.
            var haveControls = false;
            if (shouldIncludeDeviceLayout && m_Mode != InputControlPicker.Mode.PickDevice)
            {
                var childCountBefore = deviceItem.children.Count();

                // The keyboard is special in that we want to allow binding by display name (i.e. character
                // generated by a key) instead of only by physical key location. Also, we want to give an indication
                // of which specific key an entry refers to by taking the current keyboard layout into account.
                //
                // So what we do is add an extra level to the keyboard where key's can be bound by character
                // according to the current layout. And in the top level of the keyboard we display keys with
                // both physical and logical names.
                if (layout.type == typeof(Keyboard) && Keyboard.current != null)
                {
                    var byLocationGroup = new AdvancedDropdownItem("By Location of Key (Using US Layout)");
                    var byCharacterGroup = new AdvancedDropdownItem("By Character Mapped to Key");

                    deviceItem.AddChild(byLocationGroup);
                    deviceItem.AddChild(byCharacterGroup);

                    var keyboard = Keyboard.current;

                    AddCharacterKeyBindingsTo(byCharacterGroup, keyboard);
                    AddPhysicalKeyBindingsTo(byLocationGroup, keyboard, searchable);
                }
                else
                {
                    AddControlTreeItemsRecursive(layout, deviceItem, layout.name, null, searchable);
                }

                haveControls = deviceItem.children.Count() != childCountBefore;
            }

            // Add child items.
            var isFirstChild = true;
            foreach (var childLayout in childLayouts)
            {
                if (!ShouldIncludeDeviceLayout(childLayout))
                    continue;

                if (isFirstChild && haveControls)
                    deviceItem.AddSeparator("More Specific " + deviceItem.name.GetPlural());
                isFirstChild = false;

                AddDeviceTreeItemRecursive(childLayout, deviceItem, searchable && !childLayout.isGenericTypeOfDevice);
            }
        }

        private void AddControlTreeItemsRecursive(InputControlLayout layout, DeviceDropdownItem parent,
            string device, string usage, bool searchable, ControlDropdownItem parentControl = null)
        {
            foreach (var control in layout.controls.OrderBy(a => a.name))
            {
                if (control.isModifyingChildControlByPath)
                    continue;

                // Skip variants except the default variant and variants dictated by the layout itself.
                if (!control.variants.IsEmpty() && control.variants != InputControlLayout.DefaultVariant
                    && (layout.variants.IsEmpty() || !InputControlLayout.VariantsMatch(layout.variants, control.variants)))
                {
                    continue;
                }

                var child = new ControlDropdownItem(parentControl, control.name, control.displayName,
                    device, usage, searchable);
                child.icon = EditorInputControlLayoutCache.GetIconForLayout(control.layout);

                if (LayoutMatchesExpectedControlLayoutFilter(control.layout))
                    parent.AddChild(child);

                var controlLayout = EditorInputControlLayoutCache.TryGetLayout(control.layout);
                if (controlLayout != null)
                    AddControlTreeItemsRecursive(controlLayout, parent, device, usage,
                        searchable, child);
            }

            // Add optional controls for devices.
            var optionalControls = EditorInputControlLayoutCache.GetOptionalControlsForLayout(layout.name);
            if (optionalControls.Any() && layout.isDeviceLayout)
            {
                var optionalGroup = new AdvancedDropdownItem("Optional Controls");
                foreach (var optionalControl in optionalControls)
                {
                    if (LayoutMatchesExpectedControlLayoutFilter(optionalControl.layout))
                    {
                        var child = new OptionalControlDropdownItem(optionalControl, device, usage);
                        child.icon = EditorInputControlLayoutCache.GetIconForLayout(optionalControl.layout);
                        optionalGroup.AddChild(child);
                    }
                }

                if (optionalGroup.children.Any())
                {
                    var deviceName = EditorInputControlLayoutCache.TryGetLayout(device).m_DisplayName ??
                        ObjectNames.NicifyVariableName(device);
                    parent.AddSeparator("Controls Present on More Specific " + deviceName.GetPlural());
                    parent.AddChild(optionalGroup);
                }
            }
        }

        private static void AddPhysicalKeyBindingsTo(AdvancedDropdownItem parent, Keyboard keyboard, bool searchable)
        {
            foreach (var key in keyboard.children.OfType<KeyControl>())
            {
                // If the key has a display name that differs from the key name, show it in the UI.
                var displayName = key.name;
                var keyDisplayName = key.displayName;
                if (keyDisplayName.All(x => x.IsPrintable()) && keyDisplayName != key.name)
                    displayName = $"{key.name} (Current Layout: {key.displayName})";

                var item = new ControlDropdownItem(null, key.name, displayName,
                    keyboard.layout, "", searchable);

                parent.AddChild(item);
            }
        }

        private static void AddCharacterKeyBindingsTo(AdvancedDropdownItem parent, Keyboard keyboard)
        {
            foreach (var key in keyboard.children.OfType<KeyControl>())
            {
                if (!key.keyCode.IsTextInputKey())
                    continue;

                // We can only bind to characters that can be printed.
                var displayName = key.displayName;
                if (!displayName.All(x => x.IsPrintable()))
                    continue;

                if (displayName.Contains(')'))
                    displayName = string.Join("", displayName.Select(x => "\\" + x));

                ////TODO: should be searchable; when searching, needs different display name
                var item = new ControlDropdownItem(null, $"#({displayName})", "", keyboard.layout, "", false);
                item.name = key.displayName;

                parent.AddChild(item);
            }
        }

        private bool LayoutMatchesExpectedControlLayoutFilter(string layout)
        {
            if (m_ExpectedControlType == null)
                return true;

            var layoutType = InputSystem.s_Manager.m_Layouts.GetControlTypeForLayout(new InternedString(layout));
            return m_ExpectedControlType.IsAssignableFrom(layoutType);
        }

        private bool ShouldIncludeDeviceLayout(InputControlLayout layout)
        {
            if (layout.hideInUI)
                return false;

            // By default, if a device has no (usable) controls, we don't want it listed in the control picker
            // except if we're picking devices.
            if (!layout.controls.Any(x => LayoutMatchesExpectedControlLayoutFilter(x.layout)) && m_Mode != InputControlPicker.Mode.PickDevice)
                return false;

            // If we have a device filter, see if we should ignore the device.
            if (m_ControlPathsToMatch != null && m_ControlPathsToMatch.Length > 0)
            {
                var matchesAnyInDeviceFilter = false;
                foreach (var entry in m_ControlPathsToMatch)
                {
                    if (entry == layout.name ||
                        InputControlLayout.s_Layouts.IsBasedOn(new InternedString(entry), layout.name))
                    {
                        matchesAnyInDeviceFilter = true;
                    }
                    else
                    {
                        ////FIXME: this also needs to work for full control paths and not just stuff like "<Gamepad>"
                        var expectedLayout = InputControlPath.TryGetDeviceLayout(entry);
                        if (!string.IsNullOrEmpty(expectedLayout) &&
                            (expectedLayout == layout.name ||
                             InputControlLayout.s_Layouts.IsBasedOn(new InternedString(expectedLayout), layout.name)))
                        {
                            matchesAnyInDeviceFilter = true;
                        }
                    }
                }

                if (!matchesAnyInDeviceFilter)
                    return false;
            }

            return true;
        }

        private void StartListening()
        {
            if (m_RebindingOperation == null)
                m_RebindingOperation = new InputActionRebindingExtensions.RebindingOperation();

            m_RebindingOperation
                .WithExpectedControlLayout(m_ExpectedControlLayout)
                // Require minimum actuation of 0.15f. This is after deadzoning has been applied.
                .WithMagnitudeHavingToBeGreaterThan(0.15f)
                ////REVIEW: should we exclude only the system's active pointing device?
                // With the mouse operating the UI, its cursor control is too fickle a thing to
                // bind to. Ignore mouse position and delta and clicks.
                // NOTE: We go for all types of pointers here, not just mice.
                .WithControlsExcluding("<Pointer>/position")
                .WithControlsExcluding("<Pointer>/delta")
                .WithControlsExcluding("<Pointer>/button")
                .WithControlsExcluding("<Mouse>/leftButton")
                .WithControlsExcluding("<Mouse>/scroll")
                .OnPotentialMatch(
                    operation =>
                    {
                        // We never really complete the pick but keep listening for as long as the "Interactive"
                        // button is toggled on.

                        Repaint();
                    })
                .OnCancel(
                    operation =>
                    {
                        Repaint();
                    })
                .OnApplyBinding(
                    (operation, newPath) =>
                    {
                        // This is never invoked (because we don't complete the pick) but we need it nevertheless
                        // as RebindingOperation requires the callback if we don't supply an action to apply the binding to.
                    });

            // If we have control paths to match, pass them on.
            m_RebindingOperation.WithoutControlsHavingToMatchPath();
            if (m_ControlPathsToMatch.LengthSafe() > 0)
                m_ControlPathsToMatch.Select(x => m_RebindingOperation.WithControlsHavingToMatchPath(x));

            m_RebindingOperation.Start();
        }

        private void StopListening()
        {
            m_RebindingOperation?.Cancel();
        }

        // This differs from RebindingOperation.GeneratePathForControl in that it cycles through all
        // layouts in the inheritance chain and generates a path for each one that contains the given control.
        private static IEnumerable<string> GeneratePossiblePathsForControl(InputControl control)
        {
            var builder = new StringBuilder();
            var deviceLayoutName = control.device.m_Layout;
            do
            {
                // Skip layout if it is supposed to be hidden in the UI.
                var layout = EditorInputControlLayoutCache.TryGetLayout(deviceLayoutName);
                if (layout.hideInUI)
                    continue;

                builder.Length = 0;
                yield return control.BuildPath(deviceLayoutName, builder);
            }
            while (InputControlLayout.s_Layouts.baseLayoutTable.TryGetValue(deviceLayoutName, out deviceLayoutName));
        }

        private readonly Action<string> m_OnPickCallback;
        private readonly InputControlPicker.Mode m_Mode;
        private string[] m_ControlPathsToMatch;
        private string m_ExpectedControlLayout;
        private Type m_ExpectedControlType;
        private InputActionRebindingExtensions.RebindingOperation m_RebindingOperation;

        private bool isListening => m_RebindingOperation != null && m_RebindingOperation.started;

        private class InputControlPickerGUI : AdvancedDropdownGUI
        {
            private readonly InputControlPickerDropdown m_Owner;

            public InputControlPickerGUI(InputControlPickerDropdown owner)
            {
                m_Owner = owner;
            }

            internal override void BeginDraw(EditorWindow window)
            {
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Escape)
                {
                    window.Close();
                    return;
                }

                if (m_Owner.isListening)
                {
                    // Eat key events to suppress the editor from passing them to the OS
                    // (causing beeps or menu commands being triggered).
                    if (Event.current.isKey)
                        Event.current.Use();
                }
            }

            internal override string DrawSearchFieldControl(string searchString)
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    var isListening = false;

                    using (new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(42)))
                    {
                        GUILayout.Space(4);
                        var isListeningOld = m_Owner.isListening;
                        var isListeningNew = GUILayout.Toggle(isListeningOld, "Listen",
                            EditorStyles.miniButton, GUILayout.MaxWidth(40));

                        if (isListeningOld != isListeningNew)
                        {
                            if (isListeningNew)
                            {
                                m_Owner.StartListening();
                            }
                            else
                            {
                                m_Owner.StopListening();
                                searchString = string.Empty;
                            }
                        }

                        isListening = isListeningNew;
                    }

                    ////FIXME: the search box doesn't clear out when listening; no idea why the new string isn't taking effect
                    EditorGUI.BeginDisabledGroup(isListening);
                    var newSearchString = base.DrawSearchFieldControl(isListening ? string.Empty : searchString);
                    EditorGUI.EndDisabledGroup();

                    if (isListening)
                    {
                        var rebind = m_Owner.m_RebindingOperation;
                        return "\u0017" + string.Join("\u0017",
                            rebind.candidates.SelectMany(x => GeneratePossiblePathsForControl(x).Reverse()));
                    }

                    return newSearchString;
                }
            }

            internal override void DrawItem(AdvancedDropdownItem item, string name, Texture2D icon, bool enabled,
                bool drawArrow, bool selected, bool hasSearch, bool richText = false)
            {
                if (hasSearch && item is InputControlDropdownItem viewItem)
                    name = viewItem.searchableName;

                base.DrawItem(item, name, icon, enabled, drawArrow, selected, hasSearch);
            }

            internal override void DrawFooter(AdvancedDropdownItem selectedItem)
            {
                //dun work because there is no selection
                if (selectedItem is ControlDropdownItem controlItem)
                {
                    var content = new GUIContent(controlItem.controlPath);
                    var rect = GUILayoutUtility.GetRect(content, headerStyle, GUILayout.ExpandWidth(true));
                    EditorGUI.TextField(rect, controlItem.controlPath, headerStyle);
                }
            }
        }

        private static class Styles
        {
            public static readonly GUIStyle waitingForInputLabel = new GUIStyle("WhiteBoldLabel");

            static Styles()
            {
                waitingForInputLabel.fontSize = 22;
            }
        }
    }
}
#endif // UNITY_EDITOR
