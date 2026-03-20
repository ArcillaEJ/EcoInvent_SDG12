using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using EcoInvent.BLL.Services;
using EcoInvent.Models;
using WinFormsTimer = System.Windows.Forms.Timer;

namespace EcoInvent.UI
{
    public partial class InventoryForm : Form
    {
        private readonly InventoryService _svc;
        private readonly string _role;
        private bool IsAdmin => _role == "ADMIN";

        private readonly WinFormsTimer _clockTimer = new();
        private int selectedId = 0;
        private bool _isDarkMode = false;

        // PROFESSIONAL ECO-COLOR PALETTE (SDG 12)
        private Color ClrDeepGreen = Color.FromArgb(20, 83, 45);   // Deep Forest
        private Color ClrAccentGreen = Color.FromArgb(34, 197, 94); // Emerald
        private Color ClrBgMint = Color.FromArgb(240, 253, 244);   // Soft Mint
        private Color ClrCardWhite = Color.White;
        private Color ClrTextPrimary = Color.FromArgb(17, 24, 39);
        private Color ClrTextSecondary = Color.FromArgb(107, 114, 128);
        private Color ClrBorder = Color.FromArgb(230, 230, 230);
        
        private Color ClrSidebarBg => IsAdmin ? ClrDeepGreen : Color.FromArgb(51, 65, 85);

        // Sidebar & Layout
        private Panel pnlSidebar = null!, pnlHeader = null!, pnlContainer = null!;
        private Panel viewDash = null!, viewLedger = null!, viewReports = null!;
        private TableLayoutPanel splitLedger = null!;
        private Panel pnlInputCard = null!;

        // Components
        private Label lblClock = null!, lblTitle = null!, lblSub = null!, lblKpiTotal = null!, lblKpiLow = null!, lblKpiCats = null!;
        private Chart chartDash = null!, chartRep = null!;
        private TextBox txtSearch = null!;
        private Button btnNavDash = null!, btnNavLedger = null!, btnNavRep = null!, btnAdd = null!, btnUpdate = null!, btnDelete = null!;
        private Button btnThemeToggle = null!;
        
        // Profile Components
        private Panel pnlProfile = null!;
        private Label lblProfileName = null!;
        private Button btnLogout = null!;

        public InventoryForm(InventoryService svc, string role)
        {
            InitializeComponent();
            _svc = svc;
            _role = string.IsNullOrWhiteSpace(role) ? "VIEWER" : role.Trim().ToUpperInvariant();

            SetupProfessionalLayout();
            ConfigureDataGrid();

            Shown += async (s, e) => {
                StartClock();
                Navigate("DASHBOARD");
                await RefreshSystemData();
            };
        }

        private void SetupProfessionalLayout()
        {
            Text = IsAdmin ? "EcoInvent Enterprise | System Administrator" : "EcoInvent | Inventory Viewer";
            Size = new Size(1500, 900);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = ClrBgMint;
            Font = new Font("Segoe UI", 10F);
            Controls.Clear();

            // SIDEBAR
            pnlSidebar = new Panel { 
                Dock = DockStyle.Left, 
                Width = 280, 
                BackColor = ClrSidebarBg,
                Padding = new Padding(0, 30, 0, 0) 
            };
            
            var lblLogo = new Label { Text = "ECOINVENT", Font = new Font("Segoe UI", 20F, FontStyle.Bold), ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Top, Height = 100 };
            
            btnNavRep = CreateNavBtn("📊 Sustainability Reports");
            btnNavLedger = CreateNavBtn(IsAdmin ? "📋 Inventory Ledger" : "🔍 Browse Inventory");
            btnNavDash = CreateNavBtn(IsAdmin ? "🏠 Control Dashboard" : "🏠 Overview Dashboard");

            btnNavDash.Click += (s, e) => Navigate("DASHBOARD");
            btnNavLedger.Click += (s, e) => Navigate("LEDGER");
            btnNavRep.Click += (s, e) => Navigate("REPORTS");

            btnThemeToggle = new Button { 
                Text = "🌙 DARK MODE", 
                Dock = DockStyle.Bottom, 
                Height = 60, 
                FlatStyle = FlatStyle.Flat, 
                ForeColor = Color.FromArgb(180, 255, 200), 
                Font = new Font("Segoe UI", 9F, FontStyle.Bold), 
                Cursor = Cursors.Hand 
            };
            btnThemeToggle.FlatAppearance.BorderSize = 0;
            btnThemeToggle.Click += (s, e) => ToggleProfessionalTheme();

            pnlSidebar.Controls.Add(btnThemeToggle);
            if (IsAdmin) pnlSidebar.Controls.Add(btnNavRep);
            pnlSidebar.Controls.Add(btnNavLedger);
            pnlSidebar.Controls.Add(btnNavDash);
            pnlSidebar.Controls.Add(lblLogo);

            // HEADER
            pnlHeader = new Panel { Dock = DockStyle.Top, Height = 125, BackColor = ClrCardWhite, Padding = new Padding(30, 0, 30, 0) };
            pnlHeader.Paint += (s, e) => e.Graphics.DrawLine(new Pen(ClrBorder), 0, pnlHeader.Height - 1, pnlHeader.Width, pnlHeader.Height - 1);

            lblTitle = new Label { Text = "Dashboard Overview", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = IsAdmin ? ClrDeepGreen : Color.FromArgb(51, 65, 85), AutoSize = true, Location = new Point(30, 20) };
            lblSub = new Label { 
                Text = IsAdmin ? "👑 FULL SYSTEM ADMINISTRATIVE ACCESS" : "👤 READ-ONLY INVENTORY ACCESS", 
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), 
                ForeColor = IsAdmin ? ClrAccentGreen : Color.SteelBlue, 
                AutoSize = true, 
                Location = new Point(34, 78)
            };
            
            lblClock = new Label { Font = new Font("Segoe UI", 10F, FontStyle.Bold), ForeColor = ClrDeepGreen, AutoSize = true };

            // PROFILE SECTION
            pnlProfile = new Panel { Size = new Size(250, 90), Anchor = AnchorStyles.Top | AnchorStyles.Right, BackColor = Color.Transparent };
            
            var pnlAvatar = new Panel { Size = new Size(50, 50), Location = new Point(10, 22), BackColor = IsAdmin ? ClrAccentGreen : Color.SteelBlue };
            pnlAvatar.Paint += (s, e) => {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = new GraphicsPath();
                path.AddEllipse(0, 0, pnlAvatar.Width - 1, pnlAvatar.Height - 1);
                e.Graphics.FillPath(new SolidBrush(pnlAvatar.BackColor), path);
                string initial = IsAdmin ? "A" : "V";
                using var font = new Font("Segoe UI", 14F, FontStyle.Bold);
                var size = e.Graphics.MeasureString(initial, font);
                e.Graphics.DrawString(initial, font, BrBrush(Color.White), (pnlAvatar.Width - size.Width) / 2, (pnlAvatar.Height - size.Height) / 2);
            };

            lblProfileName = new Label { 
                Text = IsAdmin ? "Administrator" : "Viewer User", 
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold), 
                ForeColor = ClrTextPrimary, 
                Location = new Point(65, 25), 
                AutoSize = true 
            };

            btnLogout = new Button { 
                Text = "🚪 Logout", 
                Font = new Font("Segoe UI", 9F, FontStyle.Bold), 
                ForeColor = Color.Crimson, 
                FlatStyle = FlatStyle.Flat, 
                Location = new Point(65, 52), 
                Size = new Size(85, 30),
                Cursor = Cursors.Hand
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += (s, e) => Logout();

            pnlProfile.Controls.Add(pnlAvatar);
            pnlProfile.Controls.Add(lblProfileName);
            pnlProfile.Controls.Add(btnLogout);

            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Controls.Add(lblSub);
            pnlHeader.Controls.Add(lblClock);
            pnlHeader.Controls.Add(pnlProfile);

            // CONTAINER
            pnlContainer = new Panel { Dock = DockStyle.Fill, Padding = new Padding(35) };
            InitializeViews();

            Controls.Add(pnlContainer);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
        }

        private Brush BrBrush(Color c) => new SolidBrush(c);

        private void Logout()
        {
            if (MessageBox.Show("Are you sure you want to logout?", "EcoInvent Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.DialogResult = DialogResult.Retry;
                this.Close();
            }
        }

        private void InitializeViews()
        {
            // DASHBOARD
            viewDash = new Panel { Dock = DockStyle.Fill, Visible = false };
            var kpiRow = new TableLayoutPanel { Dock = DockStyle.Top, Height = 140, ColumnCount = 3 };
            kpiRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            kpiRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));
            kpiRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33f));

            lblKpiTotal = CreateKpi(IsAdmin ? "GROSS STOCK" : "ITEMS IN STOCK", kpiRow, 0);
            lblKpiLow = CreateKpi(IsAdmin ? "STOCK ALERTS" : "LOW STOCK ITEMS", kpiRow, 1, true);
            lblKpiCats = CreateKpi("STOCK GROUPS", kpiRow, 2);

            var chartCard = CreateCard();
            chartCard.Dock = DockStyle.Fill; chartCard.Padding = new Padding(30);
            
            chartDash = new Chart { Dock = DockStyle.Fill, Size = new Size(100, 100), Visible = false, BackColor = Color.Transparent };
            chartDash.ChartAreas.Add(new ChartArea("Main") { BackColor = Color.Transparent });
            chartDash.Series.Add(new Series("S1") { ChartType = SeriesChartType.Doughnut });
            chartCard.Controls.Add(chartDash);
            
            viewDash.Controls.Add(chartCard);
            viewDash.Controls.Add(kpiRow);

            // LEDGER
            viewLedger = new Panel { Dock = DockStyle.Fill, Visible = false };
            splitLedger = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = IsAdmin ? 2 : 1 };
            if (IsAdmin)
            {
                splitLedger.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72f));
                splitLedger.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 28f));
            }
            else
            {
                splitLedger.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));
            }

            var gridCard = CreateCard(); gridCard.Padding = new Padding(25);
            dgvItems.Parent = gridCard; dgvItems.Dock = DockStyle.Fill;
            var searchPnl = new Panel { Dock = DockStyle.Top, Height = 60 };
            txtSearch = new TextBox { Width = 350, Location = new Point(0, 15), Font = new Font("Segoe UI", 11F), BorderStyle = BorderStyle.FixedSingle, PlaceholderText = "🔍 Find assets by name or group..." };
            txtSearch.TextChanged += (s, e) => SearchLedger();
            searchPnl.Controls.Add(txtSearch);
            gridCard.Controls.Add(searchPnl);

            splitLedger.Controls.Add(gridCard, 0, 0);

            if (IsAdmin)
            {
                pnlInputCard = CreateCard(); pnlInputCard.Padding = new Padding(25);
                BuildForm(pnlInputCard);
                splitLedger.Controls.Add(pnlInputCard, 1, 0);
            }

            viewLedger.Controls.Add(splitLedger);

            // REPORTS
            viewReports = new Panel { Dock = DockStyle.Fill, Visible = false };
            var repCard = CreateCard(); repCard.Dock = DockStyle.Fill; repCard.Padding = new Padding(40);
            var repLbl = new Label { Text = "Resource Lifecycle Analysis", Font = new Font("Segoe UI", 16F, FontStyle.Bold), ForeColor = ClrDeepGreen, Dock = DockStyle.Top, Height = 50 };
            
            chartRep = new Chart { Dock = DockStyle.Fill, Size = new Size(100, 100), Visible = false, BackColor = Color.Transparent };
            chartRep.ChartAreas.Add(new ChartArea("Rep") { BackColor = Color.Transparent });
            chartRep.Series.Add(new Series("S2") { ChartType = SeriesChartType.Bar });
            repCard.Controls.Add(chartRep); repCard.Controls.Add(repLbl);
            viewReports.Controls.Add(repCard);

            pnlContainer.Controls.Add(viewDash); pnlContainer.Controls.Add(viewLedger); pnlContainer.Controls.Add(viewReports);
        }

        private void Navigate(string target)
        {
            viewDash.Visible = (target == "DASHBOARD");
            viewLedger.Visible = (target == "LEDGER");
            viewReports.Visible = (target == "REPORTS");

            lblTitle.Text = target switch { 
                "DASHBOARD" => IsAdmin ? "System Insight Dashboard" : "Inventory Overview", 
                "LEDGER" => IsAdmin ? "Resource Inventory Ledger" : "Inventory Search", 
                "REPORTS" => "Sustainability Reporting", 
                _ => "EcoInvent" 
            };

            btnNavDash.BackColor = target == "DASHBOARD" ? ClrAccentGreen : ClrSidebarBg;
            btnNavLedger.BackColor = target == "LEDGER" ? ClrAccentGreen : ClrSidebarBg;
            btnNavRep.BackColor = target == "REPORTS" ? ClrAccentGreen : ClrSidebarBg;
            
            SafeRenderCharts();
        }

        private void SafeRenderCharts()
        {
            try { 
                if (chartDash.Parent != null && chartDash.Parent.Height > 10 && chartDash.Parent.Width > 10) 
                    chartDash.Visible = viewDash.Visible;
                    
                if (chartRep.Parent != null && chartRep.Parent.Height > 10 && chartRep.Parent.Width > 10) 
                    chartRep.Visible = viewReports.Visible;
            } catch { }
        }

        private Label CreateKpi(string t, TableLayoutPanel p, int c, bool alert = false)
        {
            var card = CreateCard(); card.Margin = new Padding(c == 0 ? 0 : 15, 0, c == 2 ? 0 : 15, 25);
            var lT = new Label { Text = t, Dock = DockStyle.Top, Height = 40, Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = IsAdmin ? ClrAccentGreen : Color.SteelBlue, TextAlign = ContentAlignment.BottomLeft, Padding = new Padding(20,0,0,0) };
            var lV = new Label { Text = "0", Dock = DockStyle.Fill, Font = new Font("Segoe UI", 28F, FontStyle.Bold), ForeColor = alert ? Color.Crimson : (IsAdmin ? ClrDeepGreen : Color.FromArgb(17, 24, 39)), TextAlign = ContentAlignment.MiddleCenter };
            card.Controls.Add(lV); card.Controls.Add(lT);
            p.Controls.Add(card, c, 0);
            return lV;
        }

        private Panel CreateCard() => new Panel { BackColor = ClrCardWhite, Dock = DockStyle.Fill };
        private Button CreateNavBtn(string t) => new Button { 
            Text = "  " + t, 
            Dock = DockStyle.Top, 
            Height = 65, 
            FlatStyle = FlatStyle.Flat, 
            ForeColor = Color.White, 
            BackColor = ClrSidebarBg, 
            TextAlign = ContentAlignment.MiddleLeft, 
            Font = new Font("Segoe UI", 10F, FontStyle.Bold), 
            Cursor = Cursors.Hand, 
            FlatAppearance = { BorderSize = 0 } 
        };

        private void BuildForm(Panel p)
        {
            int y = 20;
            var head = new Label { Name = "lblFormHeader", Text = "ASSET OPERATIONS", Font = new Font("Segoe UI", 12F, FontStyle.Bold), ForeColor = ClrDeepGreen, AutoSize = true, Location = new Point(25, y) };
            p.Controls.Add(head); y += 45;

            txtName = CreateField("Item Designation", p, ref y);
            txtCategory = CreateField("Resource Category", p, ref y);
            txtStock = CreateField("Quantity", p, ref y);
            txtReorder = CreateField("Alert Level", p, ref y);

            btnAdd = ActionBtn("ADD NEW ASSET", ClrAccentGreen, p, ref y);
            btnUpdate = ActionBtn("UPDATE SELECTED", ClrDeepGreen, p, ref y);
            btnDelete = ActionBtn("DECOMMISSION", Color.Crimson, p, ref y);

            btnAdd.Click += async (s, e) => { if (V(out int sQ, out int rL)) { await _svc.AddItemAsync(txtName.Text, txtCategory.Text, sQ, rL); await RefreshSystemData(); } };
            btnUpdate.Click += async (s, e) => { if (selectedId > 0 && V(out int sQ, out int rL)) { await _svc.UpdateItemAsync(selectedId, txtName.Text, txtCategory.Text, sQ, rL); await RefreshSystemData(); } };
            btnDelete.Click += async (s, e) => { if (selectedId > 0 && MessageBox.Show("Delete Asset?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes) { await _svc.DeleteItemAsync(selectedId); await RefreshSystemData(); } };
        }

        private TextBox CreateField(string l, Panel p, ref int y)
        {
            var lbl = new Label { Text = l, Location = new Point(25, y), ForeColor = ClrDeepGreen, Font = new Font("Segoe UI", 9F, FontStyle.Bold), AutoSize = true };
            var txt = new TextBox { Location = new Point(25, y + 22), Width = p.Width - 50, Font = new Font("Segoe UI", 11F), BorderStyle = BorderStyle.FixedSingle, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            p.Controls.Add(lbl); p.Controls.Add(txt); y += 65; return txt;
        }

        private Button ActionBtn(string t, Color bg, Panel p, ref int y)
        {
            var b = new Button { Text = t, Location = new Point(25, y), Width = p.Width - 50, Height = 45, BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9F, FontStyle.Bold), Cursor = Cursors.Hand, Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right };
            b.FlatAppearance.BorderSize = 0; p.Controls.Add(b); y += 55; return b;
        }

        private void ConfigureDataGrid()
        {
            dgvItems.BorderStyle = BorderStyle.None; dgvItems.BackgroundColor = ClrCardWhite; dgvItems.GridColor = ClrBgMint; dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect; dgvItems.RowHeadersVisible = false; dgvItems.EnableHeadersVisualStyles = false; dgvItems.RowTemplate.Height = 45; dgvItems.Columns.Clear(); dgvItems.AutoGenerateColumns = false;
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ItemId", Visible = false });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ItemName", HeaderText = "ASSET NAME", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "GROUP", Width = 160 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "CurrentStock", HeaderText = "COUNT", Width = 110 });
            dgvItems.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Status", HeaderText = "STATUS", Width = 130 });
            dgvItems.ColumnHeadersDefaultCellStyle.BackColor = IsAdmin ? ClrDeepGreen : Color.FromArgb(51, 65, 85); 
            dgvItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White; 
            dgvItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvItems.CellClick += (s, e) => { 
                if (e.RowIndex >= 0 && IsAdmin) { 
                    var i = (InventoryItemView)dgvItems.Rows[e.RowIndex].DataBoundItem; 
                    selectedId = i.ItemId; txtName.Text = i.ItemName; txtCategory.Text = i.Category; txtStock.Text = i.CurrentStock.ToString(); txtReorder.Text = i.ReorderLevel.ToString(); 
                } 
            };
        }

        private async Task RefreshSystemData()
        {
            var data = await _svc.GetItemsAsync();
            dgvItems.DataSource = null; dgvItems.DataSource = data;
            lblKpiTotal.Text = data.Count.ToString();
            lblKpiLow.Text = data.Count(i => i.CurrentStock <= i.ReorderLevel).ToString();
            lblKpiCats.Text = data.Select(i => i.Category).Distinct().Count().ToString();
            UpdateVisualMetrics(data);
            
            // Re-apply search filter after data refresh
            if (!string.IsNullOrWhiteSpace(txtSearch.Text)) SearchLedger();
        }

        private void UpdateVisualMetrics(List<InventoryItemView> data)
        {
            if (data == null || !data.Any()) return;
            var g = data.GroupBy(i => i.Category).Select(x => new { N = x.Key, V = x.Sum(s => s.CurrentStock) }).ToList();
            chartDash.SuspendLayout();
            chartDash.Series[0].Points.Clear(); 
            foreach (var v in g) chartDash.Series[0].Points.AddXY(v.N, v.V);
            chartDash.ResumeLayout();
            chartRep.SuspendLayout();
            chartRep.Series[0].Points.Clear(); 
            foreach (var v in g) chartRep.Series[0].Points.AddXY(v.N, v.V);
            chartRep.ResumeLayout();
            SafeRenderCharts();
        }

        private void ToggleProfessionalTheme()
        {
            _isDarkMode = !_isDarkMode;
            btnThemeToggle.Text = _isDarkMode ? "☀️ LIGHT MODE" : "🌙 DARK MODE";
            
            ClrBgMint = _isDarkMode ? Color.FromArgb(17, 24, 39) : Color.FromArgb(240, 253, 244);
            ClrCardWhite = _isDarkMode ? Color.FromArgb(31, 41, 55) : Color.White;
            ClrTextPrimary = _isDarkMode ? Color.White : Color.FromArgb(17, 24, 39);
            ClrTextSecondary = _isDarkMode ? Color.FromArgb(156, 163, 175) : Color.FromArgb(107, 114, 128);
            ClrBorder = _isDarkMode ? Color.FromArgb(51, 65, 85) : Color.FromArgb(230, 230, 230);
            
            pnlHeader.BackColor = ClrCardWhite; 
            this.BackColor = ClrBgMint; 
            pnlContainer.BackColor = ClrBgMint; 
            
            lblTitle.ForeColor = _isDarkMode ? Color.White : (IsAdmin ? ClrDeepGreen : Color.FromArgb(51, 65, 85)); 
            lblSub.ForeColor = _isDarkMode ? (_role == "ADMIN" ? Color.LightGreen : Color.LightBlue) : (IsAdmin ? ClrAccentGreen : Color.SteelBlue);
            lblClock.ForeColor = _isDarkMode ? Color.White : ClrDeepGreen;
            lblProfileName.ForeColor = ClrTextPrimary;

            pnlHeader.Invalidate(); 

            // Recursively update all controls in the container
            UpdateControlTheme(pnlContainer);
            
            dgvItems.BackgroundColor = ClrCardWhite; 
            dgvItems.DefaultCellStyle.BackColor = ClrCardWhite; 
            dgvItems.DefaultCellStyle.ForeColor = ClrTextPrimary;
            dgvItems.GridColor = ClrBorder;
            dgvItems.DefaultCellStyle.SelectionBackColor = _isDarkMode ? Color.FromArgb(55, 65, 81) : Color.FromArgb(243, 244, 246);
            dgvItems.DefaultCellStyle.SelectionForeColor = _isDarkMode ? Color.White : ClrDeepGreen;
        }

        private void UpdateControlTheme(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Panel p && p.Parent != pnlContainer) // Cards and nested panels
                {
                    p.BackColor = ClrCardWhite;
                }
                
                if (c is Label lbl)
                {
                    // Don't override KPI values or specific alert colors
                    if (lbl.Parent.Parent is TableLayoutPanel) continue; 
                    
                    if (lbl.Font.Bold) lbl.ForeColor = ClrTextPrimary;
                    else lbl.ForeColor = ClrTextSecondary;
                }

                if (c is TextBox txt)
                {
                    txt.BackColor = _isDarkMode ? Color.FromArgb(55, 65, 81) : Color.White;
                    txt.ForeColor = ClrTextPrimary;
                }

                if (c.HasChildren) UpdateControlTheme(c);
            }
        }

        private void StartClock() { 
            _clockTimer.Interval = 1000; 
            _clockTimer.Tick += (s, e) => { 
                lblClock.Text = DateTime.Now.ToString("dddd, MMM dd | HH:mm:ss"); 
                lblClock.Location = new Point(pnlHeader.Width - pnlProfile.Width - lblClock.Width - 20, 48); 
            }; 
            _clockTimer.Start(); 
        }

        private void SearchLedger() 
        { 
            string t = txtSearch.Text.Trim().ToLower(); 
            
            // Temporarily suspend currency manager to avoid "hiding current row" exception
            dgvItems.CurrentCell = null;
            
            foreach (DataGridViewRow r in dgvItems.Rows) 
            { 
                if (r.DataBoundItem is InventoryItemView i) 
                {
                    bool match = string.IsNullOrEmpty(t) || 
                                 i.ItemName.ToLower().Contains(t) || 
                                 i.Category.ToLower().Contains(t);
                    
                    r.Visible = match; 
                }
            } 
        }

        private bool V(out int s, out int r) { s = r = 0; return !string.IsNullOrWhiteSpace(txtName.Text) && int.TryParse(txtStock.Text, out s) && int.TryParse(txtReorder.Text, out r); }
    }
}
