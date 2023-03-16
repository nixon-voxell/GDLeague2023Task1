using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public class UnityBtnEvent : UnityEvent<LobbyPage> { }

/// <summary>
/// Using it to just store button events but maybe can be reused in future
/// </summary>