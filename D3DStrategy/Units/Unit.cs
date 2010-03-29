using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using GameEngine;
using System.IO;

namespace D3DStrategy
{
	public abstract class Unit : MovableEntity
	{
		private string name;

		private bool buildable;
		private int buildCost;
		private int buildCostLeft;
		private int buildSpeed;
		private int buildSpeedTicks;

		public enum ActionStates { Idle, Moving, Attacking, Dying };
		private ActionStates actionState;

		private int hitPoints;

		private int attackPoints;
		private Unit attackTarget;
		private double attackRange;
		private int attackSpeedTicks;
		private int attackSpeed;

		public Unit()
		{
			HitPoints = 10;
			AttackSpeed = 50;
			AttackPoints = 2;
			AttackRange = 5;
			MoveSpeed = 100;

			buildable = false;
			buildCost = 1;
			buildSpeed = 300;
		}

		public int HitPoints
		{
			get { return this.hitPoints; }
			set { this.hitPoints = value; }
		}

		public bool Buildable
		{
			get { return this.buildable; }
			set { this.buildable = value; }
		}

		// * Sæt pris og sæt pris der mangler før færdig
		public int BuildCost
		{
			get { return this.buildCost; }
			set
			{
				this.buildCost = value;
				this.buildCostLeft = value;
			}
		}

		public int BuildCostLeft
		{
			get { return this.buildCostLeft; }
			set { this.buildCostLeft = value; }
		}

		public int BuildSpeed
		{
			get { return this.buildSpeed; }
			set
			{
				this.buildSpeed = value;
				this.buildSpeedTicks = this.buildSpeed;
			}
		}

		public int BuildSpeedTicks
		{
			get { return this.buildSpeedTicks; }
			set { this.buildSpeedTicks = value; }
		}

		public int AttackPoints
		{
			get { return this.attackPoints; }
			set { this.attackPoints = value; }
		}

		public string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		public ActionStates ActionState
		{
			get { return this.actionState; }
			set { this.actionState = value; }
		}

		public Unit AttackTarget
		{
			get { return this.attackTarget; }
			set { this.attackTarget = value; }
		}

		public double AttackRange
		{
			get { return this.attackRange; }
			set { this.attackRange = value; }
		}

		public int AttackSpeed
		{
			get { return this.attackSpeed; }
			set
			{
				this.attackSpeed = value;
				this.AttackSpeedTicks = this.attackSpeed;
			}
		}

		public int AttackSpeedTicks
		{
			get { return this.attackSpeedTicks; }
			set { this.attackSpeedTicks = value; }
		}

		public Unit Clone()
		{
			return (Unit)this.MemberwiseClone();
		}
	}
}
