using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmEstadisticas : Form
    {
        private readonly AutoController _controller;
        private Panel panelPrincipal;
        private Panel panelHeader;
        private Panel panelEstadisticas;
        private Timer timerActualizacion;

        // Controles de estadísticas
        private Label lblVehiculosTotal, lblVehiculosDisponibles, lblVehiculosVendidos;
        private Label lblValorInventario, lblVentasTotal, lblIngresosVentas;
        private Label lblPromedioPrecio, lblVehiculoMasCaro, lblVehiculoMasBarato;
        private Label lblMarcaPopular, lblTipoPopular, lblUltimaVenta;
        private Label lblTiempoActualizacion;

        public FrmEstadisticas(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
            IniciarActualizacionAutomatica();
        }

        private void InitializeComponent()
        {
            this.Text = "Estadísticas en Tiempo Real - AUTOS AJH";
            this.Size = new Size(800, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            CrearInterfaz();
            ActualizarEstadisticas();
        }

        private void CrearInterfaz()
        {
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            CrearHeader();
            CrearPanelEstadisticas();

            panelPrincipal.Controls.Add(panelEstadisticas);
            panelPrincipal.Controls.Add(panelHeader);

            this.Controls.Add(panelPrincipal);
        }

        private void CrearHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 100,
                BackColor = Color.FromArgb(32, 201, 151),
                Padding = new Padding(20, 0, 20, 0)
            };

            // Título
            Label lblTitulo = new Label
            {
                Text = "📊 ESTADÍSTICAS EN TIEMPO REAL",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = 60
            };

            // Tiempo de actualización
            lblTiempoActualizacion = new Label
            {
                Text = "Última actualización: --:--:--",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.White,
                Dock = DockStyle.Bottom,
                Height = 20,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // Botones
            Panel panelBotones = new Panel
            {
                Dock = DockStyle.Right,
                Width = 200,
                Height = 60
            };

            Button btnActualizar = new Button
            {
                Text = "🔄 Actualizar",
                Size = new Size(90, 35),
                Location = new Point(5, 12),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnActualizar.FlatAppearance.BorderColor = Color.White;
            btnActualizar.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 141, 101);
            btnActualizar.Click += (s, e) => ActualizarEstadisticas();

            Button btnCerrar = new Button
            {
                Text = "❌ Cerrar",
                Size = new Size(90, 35),
                Location = new Point(105, 12),
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCerrar.FlatAppearance.BorderColor = Color.White;
            btnCerrar.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 141, 101);
            btnCerrar.Click += (s, e) => this.Close();

            panelBotones.Controls.Add(btnActualizar);
            panelBotones.Controls.Add(btnCerrar);

            panelHeader.Controls.Add(panelBotones);
            panelHeader.Controls.Add(lblTiempoActualizacion);
            panelHeader.Controls.Add(lblTitulo);
        }

        private void CrearPanelEstadisticas()
        {
            panelEstadisticas = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                AutoScroll = true
            };

            int yPos = 20;

            // ===== SECCIÓN 1: RESUMEN GENERAL =====
            yPos = CrearSeccionEstadisticas("📈 RESUMEN GENERAL", Color.FromArgb(13, 110, 253), yPos, CrearControlesResumenGeneral);

            // ===== SECCIÓN 2: INVENTARIO =====
            yPos = CrearSeccionEstadisticas("🚗 INVENTARIO", Color.FromArgb(25, 135, 84), yPos, CrearControlesInventario);

            // ===== SECCIÓN 3: VENTAS =====
            yPos = CrearSeccionEstadisticas("💰 VENTAS", Color.FromArgb(255, 193, 7), yPos, CrearControlesVentas);

            // ===== SECCIÓN 4: ANÁLISIS =====
            yPos = CrearSeccionEstadisticas("🔍 ANÁLISIS", Color.FromArgb(214, 51, 132), yPos, CrearControlesAnalisis);
        }

        private int CrearSeccionEstadisticas(string titulo, Color color, int yPos, Action<Panel, int> crearControles)
        {
            Panel panelSeccion = new Panel
            {
                Location = new Point(0, yPos),
                Size = new Size(730, 180),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            // Header de la sección
            Panel panelHeaderSeccion = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(730, 40),
                BackColor = color,
                Padding = new Padding(15, 0, 15, 0)
            };

            Label lblTituloSeccion = new Label
            {
                Text = titulo,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft
            };

            panelHeaderSeccion.Controls.Add(lblTituloSeccion);
            panelSeccion.Controls.Add(panelHeaderSeccion);

            // Crear controles específicos de la sección
            crearControles(panelSeccion, 50);

            panelEstadisticas.Controls.Add(panelSeccion);

            return yPos + 190;
        }

        private void CrearControlesResumenGeneral(Panel panel, int yInicio)
        {
            int x = 20;
            int y = yInicio;

            // Vehículos Totales
            CrearItemEstadistica(panel, "Vehículos Totales:", "lblVehiculosTotal", x, y, "0");
            CrearItemEstadistica(panel, "Disponibles:", "lblVehiculosDisponibles", x + 250, y, "0", Color.FromArgb(25, 135, 84));
            CrearItemEstadistica(panel, "Vendidos:", "lblVehiculosVendidos", x + 500, y, "0", Color.FromArgb(220, 53, 69));

            y += 40;

            // Valor e Ingresos
            CrearItemEstadistica(panel, "Valor del Inventario:", "lblValorInventario", x, y, "$0.00", Color.FromArgb(13, 110, 253));
            CrearItemEstadistica(panel, "Ingresos por Ventas:", "lblIngresosVentas", x + 250, y, "$0.00", Color.FromArgb(255, 193, 7));
            CrearItemEstadistica(panel, "Total Ventas:", "lblVentasTotal", x + 500, y, "0");
        }

        private void CrearControlesInventario(Panel panel, int yInicio)
        {
            int x = 20;
            int y = yInicio;

            // Precios
            CrearItemEstadistica(panel, "Precio Promedio:", "lblPromedioPrecio", x, y, "$0.00");
            CrearItemEstadistica(panel, "Vehículo Más Caro:", "lblVehiculoMasCaro", x + 250, y, "--", Color.FromArgb(220, 53, 69));

            y += 40;

            // Marcas y Tipos
            CrearItemEstadistica(panel, "Vehículo Más Barato:", "lblVehiculoMasBarato", x, y, "--", Color.FromArgb(25, 135, 84));
            CrearItemEstadistica(panel, "Marca Más Popular:", "lblMarcaPopular", x + 250, y, "--");

            y += 40;

            CrearItemEstadistica(panel, "Tipo Más Popular:", "lblTipoPopular", x, y, "--");
        }

        private void CrearControlesVentas(Panel panel, int yInicio)
        {
            int x = 20;
            int y = yInicio;

            // Última venta
            CrearItemEstadistica(panel, "Última Venta:", "lblUltimaVenta", x, y, "No hay ventas", Color.FromArgb(111, 66, 193));

            y += 60;

            // Espacio para futuras estadísticas de ventas
            Label lblInfoVentas = new Label
            {
                Text = "💡 Las estadísticas de ventas se actualizan automáticamente\ncada vez que se registra una nueva venta en el sistema.",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(x, y),
                Size = new Size(600, 40)
            };
            panel.Controls.Add(lblInfoVentas);
        }

        private void CrearControlesAnalisis(Panel panel, int yInicio)
        {
            int x = 20;
            int y = yInicio;

            // Información del sistema
            Label lblInfoSistema = new Label
            {
                Text = "🔄 Actualización automática cada 30 segundos\n" +
                                    "📊 Todas las estadísticas en tiempo real\n" +
                                    "💾 Datos actualizados al momento de la consulta",
                Font = new Font("Segoe UI", 9f),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(x, y),
                Size = new Size(600, 60)
            };
            panel.Controls.Add(lblInfoSistema);

            // Progreso de inventario
            y += 70;
            CrearBarraProgreso(panel, "Ocupación de Inventario:", x, y);
        }

        private void CrearItemEstadistica(Panel panel, string etiqueta, string nombreControl, int x, int y, string valorInicial, Color? colorValor = null)
        {
            // Etiqueta
            Label lblEtiqueta = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(x, y),
                Size = new Size(200, 20)
            };
            panel.Controls.Add(lblEtiqueta);

            // Valor
            Label lblValor = new Label
            {
                Name = nombreControl,
                Text = valorInicial,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = colorValor ?? Color.FromArgb(33, 37, 41),
                Location = new Point(x, y + 20),
                Size = new Size(200, 25)
            };
            panel.Controls.Add(lblValor);

            // Guardar referencia para actualización
            switch (nombreControl)
            {
                case "lblVehiculosTotal": lblVehiculosTotal = lblValor; break;
                case "lblVehiculosDisponibles": lblVehiculosDisponibles = lblValor; break;
                case "lblVehiculosVendidos": lblVehiculosVendidos = lblValor; break;
                case "lblValorInventario": lblValorInventario = lblValor; break;
                case "lblVentasTotal": lblVentasTotal = lblValor; break;
                case "lblIngresosVentas": lblIngresosVentas = lblValor; break;
                case "lblPromedioPrecio": lblPromedioPrecio = lblValor; break;
                case "lblVehiculoMasCaro": lblVehiculoMasCaro = lblValor; break;
                case "lblVehiculoMasBarato": lblVehiculoMasBarato = lblValor; break;
                case "lblMarcaPopular": lblMarcaPopular = lblValor; break;
                case "lblTipoPopular": lblTipoPopular = lblValor; break;
                case "lblUltimaVenta": lblUltimaVenta = lblValor; break;
            }
        }

        private void CrearBarraProgreso(Panel panel, string etiqueta, int x, int y)
        {
            Label lblEtiqueta = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(73, 80, 87),
                Location = new Point(x, y),
                Size = new Size(200, 20)
            };
            panel.Controls.Add(lblEtiqueta);

            // Barra de progreso simulada
            Panel panelBarraBase = new Panel
            {
                Location = new Point(x, y + 25),
                Size = new Size(300, 20),
                BackColor = Color.FromArgb(233, 236, 239),
                BorderStyle = BorderStyle.FixedSingle
            };

            Panel panelBarraProgreso = new Panel
            {
                Name = "panelBarraProgreso",
                Location = new Point(0, 0),
                Size = new Size(0, 18),
                BackColor = Color.FromArgb(32, 201, 151)
            };

            panelBarraBase.Controls.Add(panelBarraProgreso);
            panel.Controls.Add(panelBarraBase);

            // Porcentaje
            Label lblPorcentaje = new Label
            {
                Name = "lblPorcentajeInventario",
                Text = "0%",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(x + 310, y + 25),
                Size = new Size(50, 20)
            };
            panel.Controls.Add(lblPorcentaje);
        }

        private void IniciarActualizacionAutomatica()
        {
            timerActualizacion = new Timer
            {
                Interval = 30000 // 30 segundos
            };
            timerActualizacion.Tick += (s, e) => ActualizarEstadisticas();
            timerActualizacion.Start();
        }

        private void ActualizarEstadisticas()
        {
            try
            {
                var autos = _controller.Autos;
                var autosDisponibles = _controller.ObtenerAutosDisponibles();
                var ventas = _controller.Ventas;

                // ===== ESTADÍSTICAS BÁSICAS =====
                lblVehiculosTotal.Text = autos.Count.ToString();
                lblVehiculosDisponibles.Text = autosDisponibles.Count.ToString();
                lblVehiculosVendidos.Text = (autos.Count - autosDisponibles.Count).ToString();
                lblValorInventario.Text = $"${_controller.ValorTotalInventario():N2}";
                lblVentasTotal.Text = ventas.Count.ToString();
                lblIngresosVentas.Text = $"${ventas.Sum(v => v.PrecioFinal):N2}";

                // ===== ESTADÍSTICAS DE PRECIOS =====
                if (autosDisponibles.Any())
                {
                    lblPromedioPrecio.Text = $"${autosDisponibles.Average(a => a.Precio):N2}";

                    var masCaro = autosDisponibles.OrderByDescending(a => a.Precio).First();
                    lblVehiculoMasCaro.Text = $"{masCaro.Marca} {masCaro.Modelo} (${masCaro.Precio:N2})";

                    var masBarato = autosDisponibles.OrderBy(a => a.Precio).First();
                    lblVehiculoMasBarato.Text = $"{masBarato.Marca} {masBarato.Modelo} (${masBarato.Precio:N2})";
                }
                else
                {
                    lblPromedioPrecio.Text = "$0.00";
                    lblVehiculoMasCaro.Text = "N/A";
                    lblVehiculoMasBarato.Text = "N/A";
                }

                // ===== ESTADÍSTICAS DE MARCAS Y TIPOS =====
                if (autosDisponibles.Any())
                {
                    var marcaPopular = autosDisponibles.GroupBy(a => a.Marca)
                                                      .OrderByDescending(g => g.Count())
                                                      .First();
                    lblMarcaPopular.Text = $"{marcaPopular.Key} ({marcaPopular.Count()} vehículos)";

                    var tipoPopular = autosDisponibles.GroupBy(a => a.Tipo)
                                                     .OrderByDescending(g => g.Count())
                                                     .First();
                    lblTipoPopular.Text = $"{tipoPopular.Key} ({tipoPopular.Count()} vehículos)";
                }
                else
                {
                    lblMarcaPopular.Text = "N/A";
                    lblTipoPopular.Text = "N/A";
                }

                // ===== ÚLTIMA VENTA =====
                if (ventas.Any())
                {
                    var ultimaVenta = ventas.OrderByDescending(v => v.FechaVenta).First();
                    lblUltimaVenta.Text = $"{ultimaVenta.Cliente} - ${ultimaVenta.PrecioFinal:N2}";
                }
                else
                {
                    lblUltimaVenta.Text = "No hay ventas registradas";
                }

                // ===== ACTUALIZAR BARRA DE PROGRESO =====
                ActualizarBarraProgreso();

                // ===== ACTUALIZAR TIEMPO =====
                lblTiempoActualizacion.Text = $"Última actualización: {DateTime.Now:HH:mm:ss}";

                // ===== ACTUALIZAR TÍTULO DE LA VENTANA =====
                this.Text = $"Estadísticas - {autosDisponibles.Count} vehículos disponibles - AUTOS AJH";

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar estadísticas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ActualizarBarraProgreso()
        {
            var autos = _controller.Autos;
            var autosDisponibles = _controller.ObtenerAutosDisponibles();

            if (autos.Any())
            {
                double porcentaje = ((double)autosDisponibles.Count / autos.Count) * 100;

                // Buscar y actualizar la barra de progreso
                var panelBarraProgreso = panelEstadisticas.Controls.Find("panelBarraProgreso", true).FirstOrDefault() as Panel;
                var lblPorcentaje = panelEstadisticas.Controls.Find("lblPorcentajeInventario", true).FirstOrDefault() as Label;

                if (panelBarraProgreso != null && lblPorcentaje != null)
                {
                    int anchoBarra = (int)(300 * (porcentaje / 100));
                    panelBarraProgreso.Size = new Size(anchoBarra, 18);
                    lblPorcentaje.Text = $"{porcentaje:N1}%";

                    // Cambiar color según el porcentaje
                    panelBarraProgreso.BackColor = porcentaje >= 70 ? Color.FromArgb(40, 167, 69) :
                                                  porcentaje >= 40 ? Color.FromArgb(255, 193, 7) :
                                                  Color.FromArgb(220, 53, 69);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            timerActualizacion?.Stop();
            timerActualizacion?.Dispose();
        }
    }
}