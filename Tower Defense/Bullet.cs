using System.Numerics;
using Raylib_CsLo;

namespace Tower_Defense
{
    internal class Bullet
    {
        Vector2 position;
        Enemy target;
        float bulletSpeed;
        int bulletSize;
        bool bulletHit;

        public Bullet(Vector2 position, float bulletSpeed, int bulletSize, Enemy enemy)
        {
            this.position = position;
            this.bulletSpeed = bulletSpeed;
            this.bulletSize = bulletSize;
            target = enemy;
        }

        public void Update()
        {
            MoveTowards();
            CollisionCheck();
        }

        public void Draw()
        {
            Raylib.DrawRectangle((int)position.X, (int)position.Y, bulletSize, bulletSize, Raylib.WHITE);
        }

        /// <summary>
        /// Moves towards the target
        /// TODO: Add an algorithm that goes straight towards enemy instead of doing an L shape
        /// </summary>
        private void MoveTowards()
        {
            if (!target.IsDead())
            {
                if (position.X < target.GetPosition().X)
                {
                    position.X += bulletSpeed;
                }
                else if (position.X > target.GetPosition().X)
                {
                    position.X -= bulletSpeed;
                }
                if (position.Y < target.GetPosition().Y)
                {
                    position.Y += bulletSpeed;
                }
                else if (position.Y > target.GetPosition().Y)
                {
                    position.Y -= bulletSpeed;
                }
            }
        }

        /// <summary>
        /// Checks if bullet is on enemy
        /// </summary>
        private void CollisionCheck()
        {
            Rectangle bulletRec = new Rectangle(position.X, position.Y, bulletSize, bulletSize);
            Rectangle enemyRec = new Rectangle(target.GetPosition().X, target.GetPosition().Y, target.GetSize().X, target.GetSize().Y);
            if (Raylib.CheckCollisionRecs(bulletRec, enemyRec))
            {
                target.Died();
                bulletHit = true;
            }
            else if (target.IsDead())
            {
                bulletHit = true;
            }
        }

        public bool HasHit()
        {
            return bulletHit;
        }
    }
}
