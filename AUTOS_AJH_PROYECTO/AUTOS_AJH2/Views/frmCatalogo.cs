using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmCatalogo : Form
    {
        private readonly AutoController _controller;
        private DataGridView dgvAutos;

        public FrmCatalogo(AutoController controller)
        {
            _controller = controller;
            BuildForm();
            LoadData();
        }

        private void BuildForm()
        {
            // Configuración básica del formulario
            this.Text = "Catálogo de Vehículos - AUTOS AJH";
            this.Size = new Size(100, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            CreateHeader();
            CreateDataGridView();
            CreateControls();
        }

        private void CreateHeader()
        {
            Panel headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(20)
            };

            Label titleLabel = new Label
            {
                Text = "🚗 CATÁLOGO DE VEHÍCULOS",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            headerPanel.Controls.Add(titleLabel);
            this.Controls.Add(headerPanel);
        }

        private void CreateDataGridView()
        {
            dgvAutos = new DataGridView
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 120),
                Size = new Size(1000, 460),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            ConfigureColumns();
            this.Controls.Add(dgvAutos);
        }

        private void ConfigureColumns()
        {
            dgvAutos.Columns.Clear();

            // Columna Código
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Codigo",
                HeaderText = "CÓDIGO",
                Width = 80
            });

            // Columna Marca
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Marca",
                HeaderText = "MARCA",
                Width = 100
            });

            // Columna Modelo
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Modelo",
                HeaderText = "MODELO",
                Width = 120
            });

            // Columna Año
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Ano",
                HeaderText = "AÑO",
                Width = 70
            });

            // Columna Tipo
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Tipo",
                HeaderText = "TIPO",
                Width = 80
            });

            // Columna Precio
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Precio",
                HeaderText = "PRECIO ($)",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "C2",
                    Alignment = DataGridViewContentAlignment.MiddleRight
                }
            });

            // Columna Color
            dgvAutos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Color",
                HeaderText = "COLOR",
                Width = 80
            });

            // Estilizar encabezados
            dgvAutos.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle()
            {
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
        }

        private void CreateControls()
        {
            Panel controlsPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(10)
            };

            // Botones de ordenamiento
            Label sortLabel = new Label
            {
                Text = "Ordenar por:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(10, 20),
                Size = new Size(80, 20)
            };

            Button btnSortMarca = new Button
            {
                Text = "🏭 Marca",
                Location = new Point(100, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSortMarca.Click += (s, e) => SortCars("Marca");

            Button btnSortModelo = new Button
            {
                Text = "🚗 Modelo",
                Location = new Point(190, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSortModelo.Click += (s, e) => SortCars("Modelo");

            Button btnSortAno = new Button
            {
                Text = "📅 Año",
                Location = new Point(280, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSortAno.Click += (s, e) => SortCars("Ano");

            Button btnSortPrecio = new Button
            {
                Text = "💰 Precio",
                Location = new Point(370, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSortPrecio.Click += (s, e) => SortCars("Precio");

            // Búsqueda simple
            TextBox searchBox = new TextBox
            {
                Location = new Point(500, 18),
                Size = new Size(150, 20),
                Text = "Buscar...",
                ForeColor = Color.Gray
            };
            searchBox.GotFocus += (s, e) =>
            {
                if (searchBox.Text == "Buscar...")
                {
                    searchBox.Text = "";
                    searchBox.ForeColor = Color.Black;
                }
            };
            searchBox.LostFocus += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(searchBox.Text))
                {
                    searchBox.Text = "Buscar...";
                    searchBox.ForeColor = Color.Gray;
                }
            };

            Button btnSearch = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(660, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += (s, e) => SearchCars(searchBox.Text);

            Button btnClear = new Button
            {
                Text = "🔄 Limpiar",
                Location = new Point(750, 15),
                Size = new Size(80, 30),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += (s, e) => LoadData();

            controlsPanel.Controls.AddRange(new Control[] {
                sortLabel, btnSortMarca, btnSortModelo, btnSortAno, btnSortPrecio,
                searchBox, btnSearch, btnClear
            });

            this.Controls.Add(controlsPanel);
        }

        private void LoadData()
        {
            try
            {
                var autos = _controller.ObtenerAutosDisponibles();
                dgvAutos.DataSource = autos;

                // Mostrar mensaje de éxito
                this.Text = $"Catálogo - {autos.Count} vehículos - AUTOS AJH";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SortCars(string criterion)
        {
            var autos = _controller.ObtenerAutosDisponibles();

            switch (criterion)
            {
                case "Marca":
                    dgvAutos.DataSource = autos.OrderBy(a => a.Marca).ToList();
                    break;
                case "Modelo":
                    dgvAutos.DataSource = autos.OrderBy(a => a.Modelo).ToList();
                    break;
                case "Ano":
                    dgvAutos.DataSource = autos.OrderBy(a => a.Ano).ToList();
                    break;
                case "Precio":
                    dgvAutos.DataSource = autos.OrderBy(a => a.Precio).ToList();
                    break;
            }
        }

        private void SearchCars(string searchText)
        {
            if (searchText == "Buscar...")
                searchText = "";

            var autos = _controller.ObtenerAutosDisponibles();

            if (!string.IsNullOrEmpty(searchText))
            {
                var filtered = autos.Where(a =>
                    a.Marca.ToLower().Contains(searchText.ToLower()) ||
                    a.Modelo.ToLower().Contains(searchText.ToLower()) ||
                    a.Codigo.ToLower().Contains(searchText.ToLower())
                ).ToList();

                dgvAutos.DataSource = filtered;
                this.Text = $"Catálogo - {filtered.Count} vehículos encontrados - AUTOS AJH";
            }
            else
            {
                LoadData();
            }
        }
    }
}