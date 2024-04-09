using Raylib_CsLo;
using System.Numerics;

namespace Tower_Defense
{
    /// <summary>
    /// Troop contains the players soldiers information
    /// Position and stats.
    /// </summary>
    internal class Troop
    {
        Vector2 position;
        Vector2 troopSize;
        Enemy target;
        float bulletSpeed;
        int bulletSize;
        int damage;
        int troopCost { get; set; }
        int fireRate;
        int timeSinceLastShot = 0;

        public Troop(Vector2 position, Vector2 troopSize, int cost, float bulletSpeed, int bulletSize, int damage, int firerate)
        {
            this.position = position;
            this.troopSize = troopSize;
            troopCost = cost;
            this.bulletSpeed = bulletSpeed;
            this.bulletSize = bulletSize;
            this.damage = damage;
            this.fireRate = firerate;
        }

        /// <summary>
        /// The troops draw script
        /// </summary>
        public void Draw()
        {
            if (troopSize.X < 64 && troopSize.Y < 64)
            {
                // I wonder what these are really called in math
                float leftOverSpace = 64 - troopSize.X;
                float halfOfTheSpace = leftOverSpace / 2;
                Vector2 newposition = new Vector2(position.X + halfOfTheSpace, position.Y + halfOfTheSpace);
                Raylib.DrawRectangle((int)newposition.X, (int)newposition.Y, (int)troopSize.X, (int)troopSize.Y, Raylib.BLUE);
            }
            else
            {
                Raylib.DrawRectangle((int)position.X, (int)position.Y, (int)troopSize.X, (int)troopSize.Y, Raylib.BLUE);
            }
        }

        /// <summary>
        /// Handles troops shooting, don't know if there really is any other uses.
        /// </summary>
        public void Update()
        {
            Aim();
            Shoot();
        }

        /// <summary>
        /// Currently gets the closest enemy to player
        /// TODO: Add more aiming algorithms for example the furthest enemy
        /// </summary>
        private void Aim()
        {
            foreach (Enemy enemy in GameLoop.enemies)
            {
                if (enemy.IsDead())
                {
                    continue;
                }
                float tempDistance = (enemy.GetPosition().X + enemy.GetPosition().Y) - (position.X + position.Y);
                float lowestDistance = 0;
                if (tempDistance < lowestDistance)
                {
                    target = enemy;
                    lowestDistance = tempDistance;
                }
            }
        }

        /// <summary>
        /// The troops shooting script, probably would be good to make it overridable for different shooting mechanisms.
        /// TODO: Gets target to interpolate where to shoot.
        /// </summary>
        private void Shoot()
        {
            if (target == null)
            {
                Aim();
            }
            else
            {
                if (!target.IsDead())
                {
                    timeSinceLastShot++;
                    if (timeSinceLastShot > fireRate)
                    {
                        timeSinceLastShot = 0;
                        Bullet bullet = new Bullet(position, bulletSpeed, bulletSize, target);
                        GameLoop.bullets.Add(bullet);
                    }
                }
            }
        }

        public int GetTroopCost()
        {
            return troopCost;
        }

        public Vector2 GetTroopPosition()
        {
            return position;
        }
    }
}
