using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BruteForceExample
{
    public class LockoutManager
    {
        private int maxAttempts;
        private TimeSpan lockoutDuration;
        private int failedAttempts;
        private DateTime? lockoutEnd;

        public LockoutManager(int maxAttempts, TimeSpan lockoutDuration)
        {
            this.maxAttempts = maxAttempts;
            this.lockoutDuration = lockoutDuration;
            this.failedAttempts = 0;
            this.lockoutEnd = null;
        }

        public void RegisterFailedAttempt()
        {
            if (lockoutEnd.HasValue && DateTime.Now >= lockoutEnd.Value)
            {
                // Reset failed attempts efter lockout
                failedAttempts = 0;
                lockoutEnd = null;
            }

            failedAttempts++;

            if (failedAttempts >= maxAttempts)
            {
                lockoutEnd = DateTime.Now.Add(lockoutDuration);
                Console.WriteLine($"För många misslyckade försök. Du är nu låst ute i {lockoutDuration.TotalMinutes} minuter.");
            }
        }

        public bool IsLockedOut()
        {
            if (lockoutEnd.HasValue && DateTime.Now < lockoutEnd.Value)
            {
                return true;
            }
            return false;
        }
    }
}

