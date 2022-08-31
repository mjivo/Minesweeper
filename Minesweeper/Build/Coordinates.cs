namespace Minesweeper.Build
{
    using System;
    using Contracts;
    using Data.Uttilites.Exception_messages;

    internal class Coordinates : ICoordinates, IComparable
    {
        private int _x;
        private int _y;

        public Coordinates(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X
        {
            get => this._x; private set
            {
                if (value < 0)
                    throw new ArgumentException(string.Format(CoordinatesClassExceptions.NegativeValueForCoordinate, nameof(this.X)));
                this._x = value;
            }
        }

        public int Y
        {
            get => this._y;
            private set
            {
                if (value < 0)
                    throw new ArgumentException(string.Format(CoordinatesClassExceptions.NegativeValueForCoordinate, nameof(this.Y)));
                this._y = value;
            }
        }
        public int CompareTo(object obj)
        {
            if (obj == null)
                throw new NullReferenceException(CoordinatesClassExceptions.CompearToCanNotAcceptNullObject);
            if (obj is ICoordinates secondCoordinates)
            {
                return this.GetHashCode() - obj.GetHashCode();
                //if (secondCoordinates.X == this.X)
                //{
                //    if (secondCoordinates.Y == this.Y)
                //    {
                //        return 0;
                //    }
                //    else
                //    {
                //        return this.Y - secondCoordinates.Y;
                //    }
                //}
                //else
                //{
                //    return this.X - secondCoordinates.X;
                //}
            }
            else
            {
                throw new InvalidCastException(CoordinatesClassExceptions.CompearToCanNotAcceptAnythingThanICoordinates);
            }
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() * 13 + 17 + this.Y.GetHashCode() * 17 + 13;

        }

        public override bool Equals(object obj)
        {
            return obj is ICoordinates other && this.CompareTo(other) == 0;
        }

        public override string ToString()
        {
            return $"[{this.X},{this.Y}]";
        }
    }
}
