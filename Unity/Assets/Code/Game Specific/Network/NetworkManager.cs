namespace RhombicDodecahedron.Client.NetworkingTest
{

	using UnityEngine;
	using System.Collections;

	using ExitGames.Client.Photon;
	using ExitGames.Client.Photon.LoadBalancing;
	using ExitGames.Client.Photon.Lite;
	using System;
	using System.Text;
	using System.Collections;

	using Hashtable = ExitGames.Client.Photon.Hashtable;
	    
	public delegate void EventPlayerListChangeDelegate(GamePlayer gamePlayer);


	public class NetworkManager : LoadBalancingClient {

		// Events

		/// Keep in mind: When joining an existing room, this client does not get EvJoin for 
		/// those players already in the room! To initiate each player, go through the list of 
		/// players in the room on join.
		/// The event join for this client is called, however.
			
		public EventPlayerListChangeDelegate OnEventJoin;

		// Can be used to be notified when a player leaves the room (Photon: EvLeave).
		public EventPlayerListChangeDelegate OnEventLeave;

        /// <summary>Provides the CurrentRoom cast to ParticleRoom.</summary>
        /// <remarks>This could also be names CurrentRoom (with new keywork) but this way it better matches LocalPlayer.</remarks>
        public GameRoom LocalRoom { get { return (GameRoom)base.CurrentRoom; } }

/*

		/// <summary>Tracks the interval in which the local player should move (unless disabled).</summary>
        public TimeKeeper MoveInterval { get; set; }

        /// <summary>Tracks the interval in which the current position should be broadcasted.</summary>
        /// <remarks>This actually defines how many updates per second this player creates by position updates.</remarks>
        public TimeKeeper UpdateOthersInterval { get; set; }

        /// <summary>Tracks the interval in which PhotonPeer.DispatchIncomingCommands should be called.</summary>
        /// <remarks>Instead of dispatching incoming info every frame, this demo will do find with a slightly lower rate.</remarks>
        public TimeKeeper DispatchInterval { get; set; }

        /// <summary>Tracks the interval in which PhotonPeer.SendOutgoingCommands should be called.</summary>
        /// <remarks>You can send in fixed intervals and additionally send when some update was created (to speed up delivery).</remarks>
        public TimeKeeper SendInterval { get; set; }

*/
        /// <summary>Internally used property to get some timestamp.</summary>
        /// <remarks>Could be exchanged, if some platform doesn't provide Environment.TickCount or if more precision is needed</remarks>
        public static int Timestamp { get { return Environment.TickCount; } }
	    
	    public new Player LocalPlayer { get { return (Player)base.LocalPlayer; } }


        public NetworkManager(string masterAddress, string appId, string gameVersion) : base(masterAddress, appId, gameVersion) {
        	this.LocalPlayer.Name = "usr" + SupportClass.ThreadSafeRandom.Next() % 99;
            this.OnStateChangeAction = this.OnStateChanged;
/*
			this.DispatchInterval = new TimeKeeper(10);
            this.SendInterval = new TimeKeeper(100);
            this.MoveInterval = new TimeKeeper(500);
            this.UpdateOthersInterval = new TimeKeeper(this.MoveInterval.Interval);
*/


        }

		// Use this for initialization
		void Start () {
	        
            if (!this.Connect())
            {
                this.DebugReturn(DebugLevel.ERROR, "Can't connect to NameServer: " + this.NameServerAddress);
            }
		}

		private void OnStateChanged(ClientState clientState)
        {
            switch (clientState)
            {
                case ClientState.ConnectedToNameServer:
                    if (string.IsNullOrEmpty(this.CloudRegion))
                    {
                        this.OpGetRegions();
                    }
                    break;
                case ClientState.ConnectedToGameserver:
                    break;
                case ClientState.ConnectedToMaster:
                    // authentication concludes connecting to the master server (it sends the appId and identifies your game)
                    // when that's done, this demo asks the Master for any game. the result is handled below
                    this.OpJoinRandomRoom(null, 0);
                    break;

            }
        }

        protected internal override Player CreatePlayer(string actorName, int actorNumber, bool isLocal, Hashtable actorProperties)
        {
            return new Player(actorName, actorNumber, isLocal, actorProperties);
        }

        /// <summary>
        /// Override of the factory method used by the LoadBalancing framework (which we extend here) to create a Room instance.
        /// </summary>
        /// <remarks>
        /// While CreateParticleDemoRoom will make the server create the room, this method creates a local object to represent that room.
        /// 
        /// This method is used by a LoadBalancingClient automatically whenever this client joins or creates a room.
        /// We override it to produce a ParticleRoom which has more features like Map and GridSize.
        /// </remarks>
        protected internal override Room CreateRoom(string roomName, RoomOptions opt)
        {
            return new Room(roomName, opt);
        }

		/// <summary>This game loop should be called as often as possible - it will do it's work in intervals only.</summary>
        public void UpdateLoop()
        {
			/*
            // Dispatch means received messages are executed - one by one when you call dispatch.
            // You could also dispatch each frame!
            if (this.DispatchInterval.ShouldExecute)
            {
                while (this.loadBalancingPeer.DispatchIncomingCommands())
                {
                    // You could count dispatch calls to limit them to X, if they take too much time of a single frame
                }
                this.DispatchInterval.Reset();  // we dispatched, so reset the timer
            }
			*/
            // If the client is in a room, we might move our LocalPlayer and update others of our position
            if (this.State == ClientState.Joined)
            {
              
            	// client joined

            }
        }

        private void sendString() {

                byte eventCode = 0;
                bool sendReliable = true;

                // sends to everyone in the room instead of just the interest group
                this.loadBalancingPeer.OpRaiseEvent(eventCode, "hello", sendReliable, null);        
        }


 		/// <summary>
        /// Implementation of a callback that's used by the Photon library to update the application / game of incoming events.
        /// </summary>
        /// <remarks>
        /// When you override this method, it's very important to call base.OnEvent to keep the state.
        /// 
        /// Photon uses events to add or remove players from this client's lists. When we call base.OnEvent() 
        /// and it adds a player, we want to fetch this player afterwards, if this removes a player, this 
        /// player will be gone after base.OnEvent(). 
        /// To get the added/removed player in any case, we might have to fetch it before or after running base code.
        /// </remarks>
        /// <param name="photonEvent">The event someone (or the server) sent.</param>
		/// 

		public override void OnEvent (EventData photonEvent)
		{
			base.OnEvent (photonEvent);
			this.DebugReturn(DebugLevel.ERROR, "Received String ");
		}
		/*
		public override void OnEvent(Photon.EventData photonEvent)
        {
            // most events have a sender / origin (but not all) - let's find the player sending this
            int actorNr = 0;
            Player origin = null;
            if (photonEvent.Parameters.ContainsKey(ParameterCode.ActorNr))
            {
                actorNr = (int)photonEvent[ParameterCode.ActorNr];  // actorNr (a.k.a. playerNumber / ID) of sending player
            }
			
			if (actorNr > 0)
			{
				this.LocalRoom.Players.TryGetValue(actorNr, out origin);
			}

            base.OnEvent(photonEvent);  // important to call, to keep state up to date
			
			if (actorNr > 0 && origin == null)
			{
				this.LocalRoom.Players.TryGetValue(actorNr, out origin);
			}
			
			// the list of players will only store Player references (not the derived class). simply cast:
			ParticlePlayer originatingPlayer = (ParticlePlayer)origin;

            // this demo logic doesn't handle any events from the server (that is done in the base class) so we could return here
            if (actorNr != 0 && originatingPlayer == null)
            {
                this.DebugReturn(DebugLevel.WARNING, photonEvent.Code + " ev. We didn't find a originating player for actorId: " + actorNr);
                return;
            }

            // this demo defined 2 events: Position and Color. additionally, a event is triggered when players join or leave
            switch (photonEvent.Code)
            {
                case DemoConstants.EvPosition:
                    originatingPlayer.ReadEvMove((Hashtable)photonEvent[ParameterCode.CustomEventContent]);
                    break;
                case DemoConstants.EvColor:
                    originatingPlayer.ReadEvColor((Hashtable)photonEvent[ParameterCode.CustomEventContent]);
                    break;
				
				// in this demo, we want a callback when players join or leave (so we can update their representation)
                case LiteEventCode.Join:
                    if (OnEventJoin != null) 
					{
						OnEventJoin(originatingPlayer);
					}
                    break;
                case LiteEventCode.Leave:
                    if (OnEventLeave != null) 
					{
						OnEventLeave(originatingPlayer);
					}
                    break;
            }

            UpdateVisuals = true;
    	}
        */

		
		// Update is called once per frame
		void Update() {
			UpdateLoop();
		}
	}
}
