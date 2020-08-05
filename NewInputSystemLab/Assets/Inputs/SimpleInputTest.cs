// GENERATED AUTOMATICALLY FROM 'Assets/Inputs/SimpleInputTest.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @SimpleInputTest : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @SimpleInputTest()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SimpleInputTest"",
    ""maps"": [
        {
            ""name"": ""GamePad"",
            ""id"": ""95d8c547-c685-4a40-957b-7afd3138b501"",
            ""actions"": [
                {
                    ""name"": ""North_Press"",
                    ""type"": ""Button"",
                    ""id"": ""e6929d7a-a34e-477d-b2b0-2d88e796490a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""South_Hold"",
                    ""type"": ""Button"",
                    ""id"": ""53ccadbf-3346-4b04-bfd5-fec0bd522222"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""a581360a-ede2-4584-885a-57310ece3b42"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""North_Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b223cdba-90a9-4c30-9695-e280c34ee869"",
                    ""path"": ""<XInputController>/buttonNorth"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""North_Press"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c07b0e58-37b6-4a49-a9d9-ab9ad66f5821"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""South_Hold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""669c532f-ede5-4569-8e15-d2505caa4f13"",
                    ""path"": ""<XInputController>/buttonSouth"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""South_Hold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GamePad
        m_GamePad = asset.FindActionMap("GamePad", throwIfNotFound: true);
        m_GamePad_North_Press = m_GamePad.FindAction("North_Press", throwIfNotFound: true);
        m_GamePad_South_Hold = m_GamePad.FindAction("South_Hold", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // GamePad
    private readonly InputActionMap m_GamePad;
    private IGamePadActions m_GamePadActionsCallbackInterface;
    private readonly InputAction m_GamePad_North_Press;
    private readonly InputAction m_GamePad_South_Hold;
    public struct GamePadActions
    {
        private @SimpleInputTest m_Wrapper;
        public GamePadActions(@SimpleInputTest wrapper) { m_Wrapper = wrapper; }
        public InputAction @North_Press => m_Wrapper.m_GamePad_North_Press;
        public InputAction @South_Hold => m_Wrapper.m_GamePad_South_Hold;
        public InputActionMap Get() { return m_Wrapper.m_GamePad; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamePadActions set) { return set.Get(); }
        public void SetCallbacks(IGamePadActions instance)
        {
            if (m_Wrapper.m_GamePadActionsCallbackInterface != null)
            {
                @North_Press.started -= m_Wrapper.m_GamePadActionsCallbackInterface.OnNorth_Press;
                @North_Press.performed -= m_Wrapper.m_GamePadActionsCallbackInterface.OnNorth_Press;
                @North_Press.canceled -= m_Wrapper.m_GamePadActionsCallbackInterface.OnNorth_Press;
                @South_Hold.started -= m_Wrapper.m_GamePadActionsCallbackInterface.OnSouth_Hold;
                @South_Hold.performed -= m_Wrapper.m_GamePadActionsCallbackInterface.OnSouth_Hold;
                @South_Hold.canceled -= m_Wrapper.m_GamePadActionsCallbackInterface.OnSouth_Hold;
            }
            m_Wrapper.m_GamePadActionsCallbackInterface = instance;
            if (instance != null)
            {
                @North_Press.started += instance.OnNorth_Press;
                @North_Press.performed += instance.OnNorth_Press;
                @North_Press.canceled += instance.OnNorth_Press;
                @South_Hold.started += instance.OnSouth_Hold;
                @South_Hold.performed += instance.OnSouth_Hold;
                @South_Hold.canceled += instance.OnSouth_Hold;
            }
        }
    }
    public GamePadActions @GamePad => new GamePadActions(this);
    public interface IGamePadActions
    {
        void OnNorth_Press(InputAction.CallbackContext context);
        void OnSouth_Hold(InputAction.CallbackContext context);
    }
}
