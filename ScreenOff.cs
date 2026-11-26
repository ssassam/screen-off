// Shows a 10-second countdown (Esc cancels), then locks the session and turns the monitor off.
// Any key (e.g. Enter) wakes the monitor.
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

class ScreenOff : Form
{
    const int CountdownSeconds = 10;

    [DllImport("user32.dll")]
    static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern bool LockWorkStation();

    [DllImport("user32.dll")]
    static extern bool RegisterHotKey(IntPtr hWnd, int id, uint modifiers, uint vk);

    [DllImport("user32.dll")]
    static extern bool UnregisterHotKey(IntPtr hWnd, int id);

    const int WM_HOTKEY = 0x0312;
    const int EscHotKeyId = 1;

    int remaining = CountdownSeconds;
    readonly Label number = new Label();
    readonly Timer timer = new Timer();

    ScreenOff()
    {
        FormBorderStyle = FormBorderStyle.None;
        WindowState = FormWindowState.Maximized;
        TopMost = true;
        ShowInTaskbar = false;
        BackColor = Color.FromArgb(20, 18, 48);
        Opacity = 0.9;
        KeyPreview = true;

        number.Dock = DockStyle.Fill;
        number.TextAlign = ContentAlignment.MiddleCenter;
        number.ForeColor = Color.FromArgb(254, 240, 138);
        number.Font = new Font("Segoe UI", 160, FontStyle.Bold);
        number.Text = remaining.ToString();

        var hint = new Label();
        hint.Dock = DockStyle.Bottom;
        hint.Height = 120;
        hint.TextAlign = ContentAlignment.MiddleCenter;
        hint.ForeColor = Color.Gainsboro;
        hint.Font = new Font("Segoe UI", 20);
        hint.Text = "Locking and turning screen off…   Esc or click to cancel";

        Controls.Add(number);
        Controls.Add(hint);

        KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };
        number.Click += (s, e) => Close();
        hint.Click += (s, e) => Close();
        Shown += (s, e) =>
        {
            RegisterHotKey(Handle, EscHotKeyId, 0, (uint)Keys.Escape); // Esc works even without focus
            Activate();
        };
        FormClosed += (s, e) => UnregisterHotKey(Handle, EscHotKeyId);
        timer.Interval = 1000;
        timer.Tick += (s, e) => Tick();
        timer.Start();
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_HOTKEY && (int)m.WParam == EscHotKeyId)
        {
            timer.Stop();
            Close();
            return;
        }
        base.WndProc(ref m);
    }

    void Tick()
    {
        remaining--;
        if (remaining > 0)
        {
            number.Text = remaining.ToString();
            return;
        }
        timer.Stop();
        Hide();
        LockWorkStation();
        System.Threading.Thread.Sleep(1000); // let the lock screen appear first
        // HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, 2 = off
        PostMessage((IntPtr)0xFFFF, 0x0112, (IntPtr)0xF170, (IntPtr)2);
        Close();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.Run(new ScreenOff());
    }
}
