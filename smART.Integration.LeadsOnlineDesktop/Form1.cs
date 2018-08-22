using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using smART.Integration.LeadsOnline;

namespace smART.Integration.LeadsOnlineDesktop {
  public partial class frmLeadsOnline : Form {
    public frmLeadsOnline() {
      InitializeComponent();
    }

    private static bool runningJob;

    private void btnClose_Click(object sender, EventArgs e) {
      Application.Exit();
    }

    private void btnPost_Click(object sender, EventArgs e) {

      try {
        if (!runningJob) {
          Cursor.Current = Cursors.WaitCursor;
          runningJob = true;
          LeadsOnlineServiceManger leadsServiceManger = new LeadsOnlineServiceManger();
          int count = leadsServiceManger.PostTickets();
           textBox1.Text=  string.Format("{0}{1} ticket(s) posted on leads at {2}.", System.Environment.NewLine, count, DateTime.Now.ToString());
        }
      }
      catch (Exception ex) {
        ExceptionHandler.HandleException(ex);
      }
      finally {
        runningJob = false;
        Cursor.Current = Cursors.Default;
      }
    }

  }
}
