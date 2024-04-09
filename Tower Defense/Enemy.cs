using Raylib_CsLo;
using System.Numerics;

namespace Tower_Defense
{
    /// <summary>
    /// Not implemented yet
    /// </summary>
    internal class Enemy
    {
        Vector2 position;
        Vector2 enemySize;
        float speed;
        int hp;
        bool isDead = false;
        public Enemy(Vector2 position, Vector2 enemySize, float speed, int hp)
        {
            this.position = position;
            this.enemySize = enemySize;
            this.speed = speed;
            this.hp = hp;
        }

        public void Update()
        {
            // TODO: Path detection for more advanced levels.
            moveRight();
            if (isDead)
            {
                Death();
            }
        }

        public void Draw()
        {
            if (enemySize.X < 64 && enemySize.Y < 64)
            {
                // I wonder what these are really called in math
                float leftOverSpace = 64 - enemySize.X;
                float halfOfTheSpace = leftOverSpace / 2;
                Vector2 newposition = new Vector2(position.X + halfOfTheSpace, position.Y + halfOfTheSpace);
                Raylib.DrawRectangle((int)newposition.X, (int)newposition.Y, (int)enemySize.X, (int)enemySize.Y, Raylib.RED);
            }
            else
            {
                Raylib.DrawRectangle((int)position.X, (int)position.Y, (int)enemySize.X, (int)enemySize.Y, Raylib.RED);
            }
        }

        private void Death()
        {
            GameLoop.enemies.Remove(this);
        }

        private void moveLeft()
        {
            position.X += speed * -1;
        }
        private void moveRight()
        {
            position.X += speed * 1;
        }
        private void moveDown()
        {
            position.X += speed * -1;
        }
        private void moveUp()
        {
            position.X += speed * 1;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public Vector2 GetSize()
        {
            return enemySize;
        }

        public void Died()
        {
            isDead = true;
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}
