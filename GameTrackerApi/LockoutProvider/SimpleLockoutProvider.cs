
namespace GameTrackerApi.LockoutProvider;

public class SimpleLockoutProvider
{
    private static bool lockedOut = false;
    private static int lockoutCount = 0;
    private static DateTime? lockoutTime;
    private readonly ILogger<SimpleLockoutProvider> _logger;
    
    public SimpleLockoutProvider(ILogger<SimpleLockoutProvider> logger)
    {
        _logger = logger;
    }


    public bool IsLockedOut()
    {
        if (LockoutTimeHasPassed())
        {
            ResetLockoutCount();
        }
        else
        {
            lockoutCount++;
            _logger.LogInformation("Lockout count: {LockoutCount}", lockoutCount);
            if (FailedAttemptsOverLockoutThreshold())
            {
                SetLockout();
            }
        }
        
        return lockedOut;
    }

    private void SetLockout()
    {
        lockedOut = true;
        lockoutTime = DateTime.Now;
    }

    private bool FailedAttemptsOverLockoutThreshold()
    {
        return lockoutCount > 4;
    }

    private bool LockoutTimeHasPassed()
    {
        return DateTime.Now - lockoutTime > TimeSpan.FromMinutes(5);
    }

    public void ResetLockoutCount()
    {
        lockoutCount = 0;
        lockedOut = false;
        lockoutTime = null;
    }
}