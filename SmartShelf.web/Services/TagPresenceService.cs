using SmartShelf.web.Data;
using SmartShelf.web.Models;

namespace SmartShelf.web.Services
{
    public class TagPresenceService
    {
        private readonly SmartShelfContext _context;

        public TagPresenceService(SmartShelfContext context)
        {
            _context = context;
        }

        /*
        IsTagPresent
        Description:
        Determines whether a tag is likely present based on recent reads.

        Logic:
        - Use a short sliding window
        - Combine read event count, total read count, and average RSSI
        - Treat weak signals as possible but require more evidence
        */
        public bool IsTagPresent(string epc, int windowSeconds = 3)
        {
            DateTime cutoff = DateTime.Now.AddSeconds(-windowSeconds);

            var recentReads = _context.TagReadEvent
                .Where(t => t.EPC == epc && t.Timestamp >= cutoff)
                .ToList();

            if (!recentReads.Any())
                return false;

            int readEvents = recentReads.Count;
            int totalReadCount = recentReads.Sum(t => t.ReadCount);
            double avgRssi = recentReads.Average(t => t.Rssi);

            // Strong / medium signal → high confidence presence
            if (avgRssi > -55)
            {
                return readEvents >= 3 && totalReadCount >= 6;
            }

            // Weak signal → still possible presence, but require more evidence
            if (avgRssi > -65)
            {
                return readEvents >= 4 && totalReadCount >= 8;
            }

            // this is tag noise
            return false;
        }

        /*
        UpdateCurrentState
        Description:
        Updates or creates TagCurrentState from recent TagReadEvent history.
        Also marks tags absent if they have not been seen recently.
        */
        public void UpdateCurrentState(string epc)
        {
            DateTime windowCutoff = DateTime.Now.AddSeconds(-3);
            DateTime absentCutoff = DateTime.Now.AddSeconds(-5);

            var recentReads = _context.TagReadEvent
                .Where(t => t.EPC == epc && t.Timestamp >= windowCutoff)
                .OrderByDescending(t => t.Timestamp)
                .ToList();

            var currentState = _context.TagCurrentState
                .FirstOrDefault(t => t.EPC == epc);

            // No recent reads in the short presence window
            if (!recentReads.Any())
            {
                if (currentState != null && currentState.LastSeenTimestamp < absentCutoff)
                {
                    currentState.IsPresent = false;
                    _context.SaveChanges();
                }

                return;
            }

            var latest = recentReads.First();
            bool isPresent = IsTagPresent(epc);

            if (currentState == null)
            {
                currentState = new TagCurrentState
                {
                    EPC = epc,
                    ReaderId = latest.ReaderId,
                    Antenna = latest.Antenna,
                    Rssi = latest.Rssi,
                    LastSeenTimestamp = latest.Timestamp,
                    ReadCount = latest.ReadCount,
                    Frequency = latest.Frequency,
                    IsPresent = isPresent
                };

                _context.TagCurrentState.Add(currentState);
            }
            else
            {
                currentState.ReaderId = latest.ReaderId;
                currentState.Antenna = latest.Antenna;
                currentState.Rssi = latest.Rssi;
                currentState.LastSeenTimestamp = latest.Timestamp;
                currentState.ReadCount = latest.ReadCount;
                currentState.Frequency = latest.Frequency;
                currentState.IsPresent = isPresent;
            }

            _context.SaveChanges();
        }
    }
}