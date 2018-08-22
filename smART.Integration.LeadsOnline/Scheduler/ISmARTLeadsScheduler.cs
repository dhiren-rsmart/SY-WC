using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace smART.Integration.LeadsOnline
{

  public interface  ISmARTLeadsScheduler {
    string Name {
      get;
    }

    void Start();

    void Stop();
  }
}
