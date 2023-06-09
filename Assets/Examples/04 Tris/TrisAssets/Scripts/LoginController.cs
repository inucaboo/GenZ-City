﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using Sfs2X;
using Sfs2X.Logging;
using Sfs2X.Util;
using Sfs2X.Core;
using Sfs2X.Entities;

namespace SFS2XExamples.Tris {
	public class LoginController : MonoBehaviour {

		//----------------------------------------------------------
		// UI elements
		//----------------------------------------------------------

		public InputField nameInput;
		public Button loginButton;
		public Text errorText;

		//----------------------------------------------------------
		// Private properties
		//----------------------------------------------------------

		private SmartFox sfs;

		//----------------------------------------------------------
		// Unity calback methods
		//----------------------------------------------------------

		void Awake() {
			Application.runInBackground = true;
			
			// Enable interface
			enableLoginUI(true);
		}
		
		// Update is called once per frame
		void Update() {
			if (sfs != null)
				sfs.ProcessEvents();
		}

		void OnApplicationQuit() {
			// Always disconnect before quitting
			if (sfs != null && sfs.IsConnected)
				sfs.Disconnect ();
		}

		// Disconnect from the socket when ordered by the main Panel scene
		public void Disconnect() {
			OnApplicationQuit();
		}

		//----------------------------------------------------------
		// Public interface methods for UI
		//----------------------------------------------------------

		public void OnLoginButtonClick() {
			enableLoginUI(false);
			
			// Set connection parameters
			ConfigData cfg = new ConfigData();
			cfg.Host = SFS2XExamples.Panel.Settings.ipAddress;
			cfg.Port = SFS2XExamples.Panel.Settings.port;
			cfg.Zone = SFS2XExamples.Panel.Settings.zone;
			
			// Initialize SFS2X client and add listeners
			#if !UNITY_WEBGL
			sfs = new SmartFox();
			#else
			sfs = new SmartFox(UseWebSocket.WS_BIN);
			#endif
			
			sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
			sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
			sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
			sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
			
			// Connect to SFS2X
			sfs.Connect(cfg);
		}

		//----------------------------------------------------------
		// Private helper methods
		//----------------------------------------------------------
		
		private void enableLoginUI(bool enable) {
			nameInput.interactable = enable;
			loginButton.interactable = enable;
			errorText.text = "";
		}
		
		private void reset() {
			// Remove SFS2X listeners
			// This should be called when switching scenes, so events from the server do not trigger code in this scene
			sfs.RemoveAllEventListeners();
			
			// Enable interface
			enableLoginUI(true);
		}

		//----------------------------------------------------------
		// SmartFoxServer event listeners
		//----------------------------------------------------------

		private void OnConnection(BaseEvent evt) {
			if ((bool)evt.Params["success"])
			{
				Debug.Log("SFS2X API version: " + sfs.Version);
				Debug.Log("Connection mode is: " + sfs.ConnectionMode);

				// Save reference to SmartFox instance; it will be used in the other scenes
				SmartFoxConnection.Connection = sfs;

				// Login
				sfs.Send(new Sfs2X.Requests.LoginRequest(nameInput.text));
			}
			else
			{
				// Remove SFS2X listeners and re-enable interface
				reset();

				// Show error message
				errorText.text = "Connection failed; is the server running at all?";
			}
		}
		
		private void OnConnectionLost(BaseEvent evt) {
			// Remove SFS2X listeners and re-enable interface
			reset();

			string reason = (string) evt.Params["reason"];

			if (reason != ClientDisconnectionReason.MANUAL) {
				// Show error message
				errorText.text = "Connection was lost; reason is: " + reason;
			}
		}
		
		private void OnLogin(BaseEvent evt) {
			// Remove SFS2X listeners and re-enable interface
			reset();

			// Load lobby scene
			//Application.LoadLevel("Lobby");
			SceneManager.LoadScene("04 TrisLobby");
		}
		
		private void OnLoginError(BaseEvent evt) {
			// Disconnect
			sfs.Disconnect();

			// Remove SFS2X listeners and re-enable interface
			reset();
			
			// Show error message
			errorText.text = "Login failed: " + (string) evt.Params["errorMessage"];
		}
	}
}