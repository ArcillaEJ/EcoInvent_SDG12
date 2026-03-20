using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using EcoInvent.BLL.Services;

namespace EcoInvent.UI
{
    public class LoginForm : Form
    {
        private readonly AuthService _authService;
        private TextBox txtUsername = null!;
        private TextBox txtPassword = null!;
        private Button btnLogin = null!;

        // BRAND COLORS (3 SHADES OF GREEN)
        private readonly Color PrimaryGreen = Color.FromArgb(5, 46, 22);   // Deep Emerald
        private readonly Color AccentGreen = Color.FromArgb(34, 197, 94);  // Vibrant Green
        private readonly Color BgMint = Color.FromArgb(240, 253, 244);      // Soft Mint

        public string LoggedInRole { get; private set; } = "VIEWER";

        // For Draggable Form
        [DllImport("user32.dll")] public static extern bool ReleaseCapture();
        [DllImport("user32.dll")] public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public LoginForm(AuthService authService)
        {
            _authService = authService;
            SetupForm();
            BuildUI();
        }

        private void SetupForm()
        {
            Text = "EcoInvent Login";
            Size = new Size(450, 550);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = BgMint;
        }

        private void BuildUI()
        {
            // Custom Title Bar
            var titleBar = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = PrimaryGreen };
            titleBar.MouseDown += (s, e) => { ReleaseCapture(); SendMessage(this.Handle, 0xA1, 0x2, 0); };

            var btnClose = new Label { Text = "✕", ForeColor = Color.White, Font = new Font("Segoe UI", 12F, FontStyle.Bold), Dock = DockStyle.Right, Width = 40, TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand };
            btnClose.Click += (s, e) => Application.Exit();
            titleBar.Controls.Add(btnClose);

            // Center Card
            var card = new Panel { Size = new Size(370, 430), BackColor = Color.White, Location = new Point(40, 80) };
            card.Paint += (s, e) => {
                using var p = new Pen(Color.FromArgb(220, 220, 220), 1);
                e.Graphics.DrawRectangle(p, 0, 0, card.Width - 1, card.Height - 1);
            };

            var lblHeader = new Label { Text = "EcoInvent", Font = new Font("Segoe UI", 26F, FontStyle.Bold), ForeColor = PrimaryGreen, Dock = DockStyle.Top, Height = 80, TextAlign = ContentAlignment.MiddleCenter };
            var lblSub = new Label { Text = "Log in to manage sustainable resources", Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, Dock = DockStyle.Top, Height = 20, TextAlign = ContentAlignment.MiddleCenter };

            var content = new Panel { Dock = DockStyle.Fill, Padding = new Padding(30) };

            txtUsername = CreateStyledInput("Username", content, 20);
            txtPassword = CreateStyledInput("Password", content, 100, true);

            btnLogin = new Button
            {
                Text = "SIGN IN",
                Height = 45,
                Width = 310,
                Location = new Point(30, 200),
                BackColor = PrimaryGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Click += async (s, e) => await PerformLogin();

            content.Controls.Add(btnLogin);
            card.Controls.Add(content);
            card.Controls.Add(lblSub);
            card.Controls.Add(lblHeader);

            Controls.Add(card);
            Controls.Add(titleBar);
            
            // Footer
            var lblFooter = new Label { Text = "SDG 12: Responsible Consumption & Production", Dock = DockStyle.Bottom, Height = 40, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.FromArgb(100, 150, 120), Font = new Font("Segoe UI", 8F) };
            Controls.Add(lblFooter);
        }

        private TextBox CreateStyledInput(string label, Panel p, int y, bool isPass = false)
        {
            var lbl = new Label { Text = label, Location = new Point(30, y), ForeColor = PrimaryGreen, Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true };
            var txt = new TextBox { Location = new Point(30, y + 25), Width = 310, Font = new Font("Segoe UI", 11F), BorderStyle = BorderStyle.FixedSingle, UseSystemPasswordChar = isPass };
            p.Controls.Add(lbl); p.Controls.Add(txt);
            return txt;
        }

        private async Task PerformLogin()
        {
            btnLogin.Enabled = false;
            btnLogin.Text = "AUTHENTICATING...";
            var res = await _authService.LoginAsync(txtUsername.Text, txtPassword.Text);
            if (res.Success) { LoggedInRole = res.Role; DialogResult = DialogResult.OK; }
            else { MessageBox.Show(res.Message, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning); btnLogin.Enabled = true; btnLogin.Text = "SIGN IN"; }
        }
    }
}
