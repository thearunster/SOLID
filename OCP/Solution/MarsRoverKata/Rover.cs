﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarsRoverKata.Commands;

namespace MarsRoverKata
{
    public class Rover
    {
        public Point Location { get; set; }
        public Direction Facing { get; set; }

        private Mars Mars { get; set; }
        private static readonly Dictionary<Direction, Point> PositionalAdjustments = new Dictionary<Direction, Point>() 
        { 
            { Direction.North, new Point(0, 1) },
            { Direction.South, new Point(0, -1) },
            { Direction.East, new Point(1, 0) },
            { Direction.West, new Point(-1, 0) }
        };
        private List<Missile> Missiles { get; set; }
        private SelfDestructor SelfDestructor { get; set; }

        public Rover(Mars mars)
            :this(mars, mars.CenterOfThePlanet)// new Point(0,0))
        { }

        public Rover(Mars mars, Point landingPoint)
        {
            Mars = mars;
            Missiles = new List<Missile>
            {
                new Missile(Mars),
                new Missile(Mars),
                new Missile(Mars)
            };
            SelfDestructor = new SelfDestructor(Mars);
            LandOnMars(landingPoint);
        }

        private void LandOnMars(Point landingPoint)
        {
            if (!Mars.IsValidPosition(landingPoint))
                throw new CrashException("Doh! We tried to land on something other than the planet and the rover was destroyed!!!");
            Location = landingPoint;
            Facing = Direction.North;
        }

        public bool MoveBackward()
        {
            return Move(-1);
        }

        public bool MoveForward()
        {
            return Move(1);
        }

        private bool Move(int adjustmentFactor)
        {
            var desiredPosition = CreateDesiredPosition(adjustmentFactor);
            var newLocation = Mars.CalculateFinalPosition(Location, desiredPosition);

            var success = Location == newLocation ? false : true;

            Location = newLocation;

            return success;
        }

        private Point CreateDesiredPosition(int adjustmentFactor)
        {
            var adjustment = PositionalAdjustments[Facing] * adjustmentFactor;
            return Location + adjustment;
        }

        public bool TurnRight()
        {
            if (Facing == Direction.North)
                Facing = Direction.East;
            else if (Facing == Direction.South)
                Facing = Direction.West;
            else if (Facing == Direction.East)
                Facing = Direction.South;
            else if (Facing == Direction.West)
                Facing = Direction.North;

            return true;
        }

        public bool TurnLeft()
        {
            if (Facing == Direction.North)
                Facing = Direction.West;
            else if (Facing == Direction.South)
                Facing = Direction.East;
            else if (Facing == Direction.East)
                Facing = Direction.North;
            else if (Facing == Direction.West)
                Facing = Direction.South;
            
            return true;
        }

        public bool FireMissile()
        {
            var missileToFire = Missiles.FirstOrDefault();
            if (missileToFire == null)
            {
                return false;
            }
            missileToFire.Launch(Facing, Location);
            return true;
        }

        public bool SelfDestruct()
        {
            SelfDestructor.Launch(Facing, Location);
            return true;
        }
    }
}