using AUTOS_AJH.Controllers;
using AUTOS_AJH.Views;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmPrincipal : Form
    {
        private readonly AutoController _controller;
        private MenuStrip menuPrincipal;
        private Panel panelHeader;
        private Label lblStats;
        private Panel panelBienvenida; // Referencia para poder removerlo

        public FrmPrincipal()
        {
            _controller = new AutoController();
            BuildForm();
            MostrarBienvenida();
        }

        private void BuildForm()
        {
            // Configuración básica del formulario
            this.Text = "Sistema de Gestión de Vehículos - AUTOS AJH";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);
            this.IsMdiContainer = true; // ✅ IMPORTANTE: Esto habilita MDI

            // Crear controles
            CreateHeader();
            CreateMenu();

            // Actualizar estadísticas
            UpdateStats();
        }

        private void CreateHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(20, 0, 20, 0)
            };

            // Título
            Label lblTitulo = new Label
            {
                Text = "🏁 AUTOS AJH - CONCESIONARIA",
                Font = new Font("Segoe UI", 18f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = panelHeader.Height
            };

            // Estadísticas
            lblStats = new Label
            {
                Text = "Cargando...",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.White,
                Dock = DockStyle.Right,
                TextAlign = ContentAlignment.MiddleRight,
                Height = panelHeader.Height
            };

            panelHeader.Controls.Add(lblStats);
            panelHeader.Controls.Add(lblTitulo);
            this.Controls.Add(panelHeader);
        }

        private void CreateMenu()
        {
            menuPrincipal = new MenuStrip
            {
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Regular)
            };

            // Menú Inicio
            ToolStripMenuItem inicioMenu = new ToolStripMenuItem("🏠 Inicio");
            inicioMenu.Click += (s, e) => MostrarBienvenida();

            // Menú Catálogo
            ToolStripMenuItem catalogoMenu = new ToolStripMenuItem("📊 Catálogo");

            ToolStripMenuItem verCatalogoItem = new ToolStripMenuItem("📋 Ver Catálogo Completo");
            verCatalogoItem.Click += (s, e) => AbrirFormulario(new FrmCatalogo(_controller));

            ToolStripMenuItem agregarAutoItem = new ToolStripMenuItem("🚗 Agregar Nuevo Auto");
            agregarAutoItem.Click += (s, e) => AbrirFormulario(new FrmAgregarAuto(_controller));

            ToolStripMenuItem buscarAutoItem = new ToolStripMenuItem("🔍 Buscar Auto por Código");
            buscarAutoItem.Click += (s, e) => BuscarAutoPorCodigo();

            catalogoMenu.DropDownItems.AddRange(new ToolStripItem[] {
                verCatalogoItem, new ToolStripSeparator(), agregarAutoItem, buscarAutoItem
            });

            // Menú Ventas
            ToolStripMenuItem ventasMenu = new ToolStripMenuItem("💰 Ventas");

            ToolStripMenuItem registrarVentaItem = new ToolStripMenuItem("💳 Registrar Venta");
            registrarVentaItem.Click += (s, e) => AbrirFormulario(new frmVenta(_controller));

            ToolStripMenuItem aplicarDescuentoItem = new ToolStripMenuItem("🎯 Aplicar Descuento");
            aplicarDescuentoItem.Click += (s, e) => AbrirFormulario(new FrmDescuento(_controller));

            ToolStripMenuItem historialVentasItem = new ToolStripMenuItem("📈 Historial de Ventas");
            historialVentasItem.Click += (s, e) => AbrirFormulario(new FrmHistorialVentas(_controller));

            ventasMenu.DropDownItems.AddRange(new ToolStripItem[] {
                registrarVentaItem, new ToolStripSeparator(), aplicarDescuentoItem, historialVentasItem
            });

            // Menú Consultas
            ToolStripMenuItem consultasMenu = new ToolStripMenuItem("🔍 Consultas");

            ToolStripMenuItem consultasEspecialesItem = new ToolStripMenuItem("📊 Consultas Especiales");
            consultasEspecialesItem.Click += (s, e) => AbrirFormulario(new FrmConsultas(_controller));

            ToolStripMenuItem reportesItem = new ToolStripMenuItem("📑 Generar Reportes");
            reportesItem.Click += (s, e) => AbrirFormulario(new FrmReportes(_controller));

            consultasMenu.DropDownItems.AddRange(new ToolStripItem[] {
                consultasEspecialesItem, new ToolStripSeparator(), reportesItem
            });

            // Menú Sistema
            ToolStripMenuItem sistemaMenu = new ToolStripMenuItem("⚙️ Sistema");

            ToolStripMenuItem actualizarStatsItem = new ToolStripMenuItem("🔄 Actualizar Estadísticas");
            actualizarStatsItem.Click += (s, e) => AbrirFormulario(new FrmEstadisticas(_controller));

            ToolStripMenuItem salirItem = new ToolStripMenuItem("🚪 Salir");
            salirItem.Click += (s, e) => this.Close();

            sistemaMenu.DropDownItems.AddRange(new ToolStripItem[] {
                actualizarStatsItem, new ToolStripSeparator(), salirItem
            });

            // Agregar menús
            menuPrincipal.Items.AddRange(new ToolStripItem[] {
                inicioMenu, catalogoMenu, ventasMenu, consultasMenu, sistemaMenu
            });

            this.Controls.Add(menuPrincipal);
            this.MainMenuStrip = menuPrincipal;
        }

        // ✅ MÉTODO CORREGIDO PARA ABRIR FORMULARIOS
        private void AbrirFormulario(Form formulario)
        {
            // Remover el panel de bienvenida si existe
            RemoverPanelBienvenida();

            // Cerrar formularios hijos existentes del mismo tipo
            foreach (Form childForm in this.MdiChildren)
            {
                if (childForm.GetType() == formulario.GetType())
                {
                    childForm.Close();
                    break;
                }
            }

            // Configurar y mostrar el nuevo formulario
            formulario.MdiParent = this;
            formulario.WindowState = FormWindowState.Maximized;
            formulario.Show();
            formulario.BringToFront(); // ✅ IMPORTANTE: Traer al frente
        }

        // ✅ MÉTODO CORREGIDO PARA REMOVER BIENVENIDA
        private void RemoverPanelBienvenida()
        {
            if (panelBienvenida != null && this.Controls.Contains(panelBienvenida))
            {
                this.Controls.Remove(panelBienvenida);
                panelBienvenida.Dispose();
                panelBienvenida = null;
            }
        }

        private void MostrarBienvenida()
        {
            RemoverPanelBienvenida(); // Limpiar primero

            // Cerrar todos los formularios MDI hijos
            foreach (Form childForm in this.MdiChildren)
            {
                childForm.Close();
            }

            panelBienvenida = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(40)
            };

            Label lblBienvenida = new Label
            {
                Text = $"🏁 BIENVENIDO A AUTOS AJH\n\n" +
                                   $"📍 Sistema de Gestión de Concesionaria\n\n" +
                                   $"📊 Estadísticas Actuales:\n" +
                                   $"   • Vehículos en stock: {_controller.TotalAutosDisponibles()}\n" +
                                   $"   • Valor total del inventario: ${_controller.ValorTotalInventario():N2}\n" +
                                   $"   • Ventas registradas: {_controller.Ventas.Count}\n\n" +
                                   $"🚀 Características Principales:\n" +
                                   $"   • Gestión completa de inventario\n" +
                                   $"   • Proceso de ventas integrado\n" +
                                   $"   • Sistema de descuentos controlado\n" +
                                   $"   • Consultas especiales y reportes\n\n" +
                                   $"👤 Usuario:Administrador\n" +
                                   $"📅 Sesión: {DateTime.Now:dd/MM/yyyy HH:mm}",
                Font = new Font("Segoe UI", 12f, FontStyle.Regular),
                ForeColor = Color.FromArgb(64, 64, 64),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            panelBienvenida.Controls.Add(lblBienvenida);
            this.Controls.Add(panelBienvenida);
            panelBienvenida.BringToFront();
        }

        private void BuscarAutoPorCodigo()
        {
            // Abrir el formulario de búsqueda
            var buscarForm = new FrmBuscarAuto(_controller);
            buscarForm.ShowDialog();
        }

        private void MostrarHistorialVentas()
        {
            if (_controller.Ventas.Count == 0)
            {
                MessageBox.Show("No hay ventas registradas.", "Historial de Ventas",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string historial = "📈 HISTORIAL DE VENTAS\n\n";
            foreach (var venta in _controller.Ventas)
            {
                historial += $"🆔 Venta: {venta.Id.Substring(0, 8)}...\n" +
                           $"🚗 Auto: {venta.CodigoAuto}\n" +
                           $"💰 Total: ${venta.PrecioFinal:N2}\n" +
                           $"📅 Fecha: {venta.FechaVenta:dd/MM/yyyy}\n" +
                           $"👤 Cliente: {venta.Cliente}\n" +
                           $"────────────────────────\n";
            }

            MessageBox.Show(historial, "Historial de Ventas - AUTOS AJH",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarConsultasEspeciales()
        {
            string consultas = "📊 CONSULTAS ESPECIALES\n\n";

            var masAntiguo = _controller.ObtenerAutoMasAntiguo();
            var mayorCilindraje = _controller.ObtenerAutoMayorCilindraje();
            var precioMasBajo = _controller.ObtenerAutoPrecioMasBajo();

            consultas += $"📅 **Auto Más Antiguo:**\n" +
                       $"   {masAntiguo?.Marca} {masAntiguo?.Modelo} {masAntiguo?.Ano}\n\n" +
                       $"⚡ **Auto Mayor Cilindraje:**\n" +
                       $"   {mayorCilindraje?.Marca} {mayorCilindraje?.Modelo} - {mayorCilindraje?.Cilindraje}cc\n\n" +
                       $"💰 **Auto Precio Más Bajo:**\n" +
                       $"   {precioMasBajo?.Marca} {precioMasBajo?.Modelo} - ${precioMasBajo?.Precio:N2}";

            MessageBox.Show(consultas, "Consultas Especiales - AUTOS AJH",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MostrarReportes()
        {
            string reporte = "📑 REPORTES DEL SISTEMA\n\n" +
                           $"📊 Total Vehículos: {_controller.Autos.Count}\n" +
                           $"✅ Disponibles: {_controller.TotalAutosDisponibles()}\n" +
                           $"💰 Valor Inventario: ${_controller.ValorTotalInventario():N2}\n" +
                           $"🎯 Ventas Totales: {_controller.Ventas.Count}\n" +
                           $"📈 Ingresos por Ventas: ${_controller.Ventas.Sum(v => v.PrecioFinal):N2}\n\n" +
                           $"🕐 Generado: {DateTime.Now:dd/MM/yyyy HH:mm}";

            MessageBox.Show(reporte, "Reportes del Sistema - AUTOS AJH",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateStats()
        {
            if (lblStats != null)
            {
                lblStats.Text = $"Stock: {_controller.TotalAutosDisponibles()} | " +
                              $"Inventario: ${_controller.ValorTotalInventario():N0} | " +
                              $"Ventas: {_controller.Ventas.Count}";
            }
        }

        private void MostrarMensaje(string mensaje)
        {
            MessageBox.Show(mensaje, "AUTOS AJH",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // ✅ Esto debe estar vacío ya que creamos todo manualmente
        private void InitializeComponent()
        {
            // Este método queda vacío porque configuramos todo manualmente
        }
    }
}