// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH">
//   Exit Games GmbH, 2012
// </copyright>
// <summary>
//   The "Particle" demo is a load balanced and Photon Cloud compatible "coding" demo.
//   The focus is on showing how to use the Photon features without too much "game" code cluttering the view.
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

namespace RhombicDodecahedron.Client.NetworkingTest
{
    using ExitGames.Client.Photon;
    using ExitGames.Client.Photon.LoadBalancing;
    using System.Collections;
    using Hashtable = ExitGames.Client.Photon.Hashtable;
    
    /// <summary>
    /// Extends Player with some Particle Demo specific properties and methods.
    /// </summary>
    /// <remarks>
    /// Instances of this class are created by GameLogic.CreatePlayer.
    /// There's a GameLogic.LocalPlayer field, that represents this user's player (in the room).
    /// 
    /// This class does not make use of networking directly. It's updated by incoming events but
    /// the actual sending and receiving is handled in GameLogic.
    /// 
    /// The WriteEv* and ReadEv* methods are simple ways to create event-content per player. 
    /// Only the LocalPlayer per client will actually send data. This is used to update the remote
    /// clients of position (and color, etc). 
    /// Receiving clients identify the corresponding Player and call ReadEv* to update that 
    /// (remote) player.
    /// Read the remarks in WriteEvMove.
    /// </remarks>
    public class GamePlayer : Player
    {
        private int LastUpdateTimestamp { get; set; }
        //public int UpdateAge { get { return GameLogic.Timestamp - this.LastUpdateTimestamp; } }

        public GamePlayer(string name, int actorID, bool isLocal, Hashtable actorProperties) : base(name, actorID, isLocal, actorProperties)
        {
      
        }

        /// <summary>
        /// Converts the player info into a string.
        /// </summary>
        /// <returns>String showing basic info about this player.</returns>
        public override string ToString()
        {
            return this.ID + "'" + this.Name + "':" + this.GetGroup() + " PlayerProps: " + SupportClass.DictionaryToString(this.CustomProperties);
        }

  		/// <summary>
        /// Gets the "Interest Group" this player is in, based on it's position (in this demo).
        /// </summary>
        /// <returns>The group id this player is in.</returns>
        public byte GetGroup()
        {
			/*
            GameRoom pr = this.RoomReference as GameRoom;
            if (pr != null)
            {
                return pr.GetGroup(this.PosX, this.PosY);
            }
			*/
            return 0;
        }

    }
}
