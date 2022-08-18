using System;

namespace Minesweeper.Build.Contracts
{
    internal interface ICoordinates /*: IComparable, IEquatable<ICoordinates>*/
    {
        public int X { get; }
        public int Y { get; }
    }
}
