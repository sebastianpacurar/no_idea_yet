using System.Linq;
using UnityEngine;

namespace Utils {
    public static class CollisionUtils {
        // return true if collision is on bottom
        public static bool IsCollisionBottom(Collision2D collision) {
            var average = CalculateAverageContactNormal(collision);
            return average.y > 0;
        }

        // return true if collision is on top
        public static bool IsCollisionTop(Collision2D collision) {
            var average = CalculateAverageContactNormal(collision);
            return average.y < 0;
        }


        // return true is collision is on left or right
        public static bool IsCollisionSideways(Collision2D collision) {
            var average = CalculateAverageContactNormal(collision);
            var collisionLeft = average.x > 0f;
            var collisionRight = average.x < 0f;

            return collisionLeft || collisionRight;
        }


        // return the average of Collision ContactPoint2D normal values  
        private static Vector2 CalculateAverageContactNormal(Collision2D collision) {
            // get the sum of all contact.normal values from contacts
            var average = collision.contacts.Aggregate(Vector2.zero, (current, contact) => current + contact.normal);

            // divide sum to the length to get the average
            average /= collision.contacts.Length;
            return average.normalized;
        }
    }
}