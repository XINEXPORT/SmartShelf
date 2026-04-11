using System.Collections.Generic;
using System.Linq;
using SmartShelf.web.Models;

namespace SmartShelf.web.Services
{
    /*
    TagPresenceService
    Description:
    Provides logic to determine whether a tag is present
    based on a set of read events from the current scan.

    Design:
    - Stateless (no DbContext)
    - Operates only on in-memory read data
    - Intended to be used inside controller after a scan
    */
    public class TagPresenceService
    {
        /*
        IsTagPresent
        Description:
        Determines whether a tag is likely present based on
        the reads collected in the current scan.

        Logic:
        - Use total read count as primary signal
        - Use average RSSI to classify signal strength
        - Require higher read counts for weaker signals
        - Filter out weak/noisy reads

        Parameters:
        - recentReads: list of read events for a single EPC from current scan

        Returns:
        - true if tag is considered present
        - false if tag is considered absent/noise
        */
        public bool IsTagPresent(List<TagReadEvent> recentReads)
        {
            if (recentReads == null || !recentReads.Any())
                return false;

            int totalReadCount = recentReads.Sum(t => t.ReadCount);
            double avgRssi = recentReads.Average(t => t.Rssi);

            // Strong / medium signal → high confidence
            if (avgRssi > -55)
            {
                return totalReadCount >= 6;
            }

            // Weaker signal → require more evidence
            if (avgRssi > -65)
            {
                return totalReadCount >= 8;
            }

            // Very weak signal → likely noise
            return false;
        }
    }
}