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
    using ExitGames.Client.Photon.LoadBalancing;
    using ExitGames.Client.Photon;
    using System.Collections;
    using Hashtable = ExitGames.Client.Photon.Hashtable;

    /// <summary>
    /// Room for the Particle demo extends Room and has additional properties and methods.
    /// </summary>
    /// <remarks>
    /// You could implement the properties (GridSize, etc) also in other ways. 
    /// Example: Override CacheProperties and whenever a property is cached, update backing fields with the new values.
    /// 
    /// The GameLogic makes sure the properties are set when we create rooms. 
    /// The values might change while the room is in use. In this demo, we change the GridSize, e.g..
    /// </remarks>
    public class GameRoom : Room
    {

        /// <summary>
        /// Uses the base constructor to initialize this ParticleRoom.
        /// </summary>
        protected internal GameRoom(string roomName, RoomOptions opt)
            : base(roomName, opt)
        {

        }
    }
}
