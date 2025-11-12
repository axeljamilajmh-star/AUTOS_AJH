using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmReportes : Form
    {
        private readonly AutoController _controller;
        private Panel panelPrincipal;
        private Panel panelHeader;
        private Panel panelOpciones;
        private Panel panelResultado;

        // Controles
        private ComboBox cmbTipoReporte;
        private Button btnGenerar, btnExportar, btnImprimir;
        private TextBox txtReporte;
        private Label lblEstadisticas;

        public FrmReportes(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Generar Reportes - AUTOS AJH";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            CrearInterfaz();
        }

        private void CrearInterfaz()
        {
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            CrearHeader();
            CrearPanelOpciones();
            CrearPanelResultado();

            panelPrincipal.Controls.Add(panelResultado);
            panelPrincipal.Controls.Add(panelOpciones);
            panelPrincipal.Controls.Add(panelHeader);

            this.Controls.Add(panelPrincipal);
        }

        private void CrearHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(102, 16, 242), // Púrpura para reportes
                Padding = new Padding(20, 0, 20, 0)
            };

            Label lblTitulo = new Label
            {
                Text = "📑 GENERAR REPORTES",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = panelHeader.Height
            };

            panelHeader.Controls.Add(lblTitulo);
        }

        private void CrearPanelOpciones()
        {
            panelOpciones = new Panel
            {
                Dock = DockStyle.Top,
                Height = 150,
                BackColor = Color.FromArgb(248, 249, 250),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(20)
            };

            // Título de sección
            Label lblTituloOpciones = new Label
            {
                Text = "Seleccione el tipo de reporte a generar:",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(20, 15),
                Size = new Size(400, 25)
            };
            panelOpciones.Controls.Add(lblTituloOpciones);

            // ComboBox para tipo de reporte
            Label lblTipoReporte = new Label
            {
                Text = "Tipo de Reporte:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(20, 55),
                Size = new Size(120, 20)
            };
            panelOpciones.Controls.Add(lblTipoReporte);

            cmbTipoReporte = new ComboBox
            {
                Location = new Point(150, 55),
                Size = new Size(300, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f)
            };
            cmbTipoReporte.Items.AddRange(new string[] {
                "📊 REPORTE GENERAL DEL SISTEMA",
                "🚗 INVENTARIO COMPLETO DE VEHÍCULOS",
                "💰 REPORTE DE VENTAS DETALLADO",
                "📈 ESTADÍSTICAS FINANCIERAS",
                "🎯 CONSULTAS ESPECIALES RESUMEN",
                "⚠️ VEHÍCULOS CON BAJA ROTACIÓN"
            });
            cmbTipoReporte.SelectedIndex = 0;
            panelOpciones.Controls.Add(cmbTipoReporte);

            // Botones
            btnGenerar = new Button
            {
                Text = "🔄 GENERAR REPORTE",
                Size = new Size(150, 40),
                Location = new Point(480, 45),
                BackColor = Color.FromArgb(13, 110, 253),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnGenerar.Click += BtnGenerar_Click;
            panelOpciones.Controls.Add(btnGenerar);

            btnExportar = new Button
            {
                Text = "💾 EXPORTAR A TEXTO",
                Size = new Size(150, 40),
                Location = new Point(480, 95),
                BackColor = Color.FromArgb(25, 135, 84),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnExportar.Click += BtnExportar_Click;
            btnExportar.Enabled = false;
            panelOpciones.Controls.Add(btnExportar);

            btnImprimir = new Button
            {
                Text = "🖨️ COPIAR AL PORTAPAPELES",
                Size = new Size(180, 40),
                Location = new Point(640, 95),
                BackColor = Color.FromArgb(111, 66, 193),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnImprimir.Click += BtnImprimir_Click;
            btnImprimir.Enabled = false;
            panelOpciones.Controls.Add(btnImprimir);

            // Estadísticas rápidas
            lblEstadisticas = new Label
            {
                Text = "Cargando estadísticas del sistema...",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(20, 95),
                Size = new Size(400, 40)
            };
            panelOpciones.Controls.Add(lblEstadisticas);

            // Cargar estadísticas iniciales
            ActualizarEstadisticasRapidas();
        }

        private void CrearPanelResultado()
        {
            panelResultado = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10)
            };

            txtReporte = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 9f),
                ScrollBars = ScrollBars.Both,
                ReadOnly = true,
                BackColor = Color.FromArgb(248, 249, 250),
                Text = "Seleccione un tipo de reporte y haga clic en 'GENERAR REPORTE' para comenzar..."
            };

            panelResultado.Controls.Add(txtReporte);
        }

        private void ActualizarEstadisticasRapidas()
        {
            try
            {
                int totalVehiculos = _controller.Autos.Count;
                int vehiculosDisponibles = _controller.TotalAutosDisponibles();
                int totalVentas = _controller.Ventas.Count;
                decimal valorInventario = _controller.ValorTotalInventario();

                lblEstadisticas.Text = $"📊 Resumen: {vehiculosDisponibles}/{totalVehiculos} vehículos disponibles | " +
                                     $"💰 Inventario: ${valorInventario:N0} | " +
                                     $"📈 Ventas: {totalVentas}";
            }
            catch (Exception)
            {
                lblEstadisticas.Text = "Error al cargar estadísticas";
            }
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            try
            {
                string tipoReporte = cmbTipoReporte.SelectedItem?.ToString();
                string reporte = "";

                switch (tipoReporte)
                {
                    case "📊 REPORTE GENERAL DEL SISTEMA":
                        reporte = GenerarReporteGeneral();
                        break;
                    case "🚗 INVENTARIO COMPLETO DE VEHÍCULOS":
                        reporte = GenerarReporteInventario();
                        break;
                    case "💰 REPORTE DE VENTAS DETALLADO":
                        reporte = GenerarReporteVentas();
                        break;
                    case "📈 ESTADÍSTICAS FINANCIERAS":
                        reporte = GenerarReporteFinanciero();
                        break;
                    case "🎯 CONSULTAS ESPECIALES RESUMEN":
                        reporte = GenerarReporteConsultas();
                        break;
                    case "⚠️ VEHÍCULOS CON BAJA ROTACIÓN":
                        reporte = GenerarReporteBajaRotacion();
                        break;
                    default:
                        reporte = "Tipo de reporte no válido.";
                        break;
                }

                txtReporte.Text = reporte;
                btnExportar.Enabled = true;
                btnImprimir.Enabled = true;

                MessageBox.Show($"✅ Reporte generado exitosamente!\n\nTipo: {tipoReporte}\nTamaño: {reporte.Length} caracteres",
                              "Reporte Generado",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al generar el reporte:\n{ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog saveDialog = new SaveFileDialog
                {
                    Filter = "Archivos de texto (*.txt)|*.txt",
                    FileName = $"Reporte_AUTOSAJH_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                };

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveDialog.FileName, txtReporte.Text);
                    MessageBox.Show($"✅ Reporte exportado exitosamente a:\n{saveDialog.FileName}",
                                  "Exportación Exitosa",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al exportar el reporte:\n{ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(txtReporte.Text);
                MessageBox.Show("✅ Reporte copiado al portapapeles.\n\nPuede pegarlo en Word, Excel o cualquier editor de texto.",
                              "Copiado al Portapapeles",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al copiar al portapapeles:\n{ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        #region Métodos de Generación de Reportes

        private string GenerarReporteGeneral()
        {
            var autos = _controller.Autos;
            var ventas = _controller.Ventas;
            var autosDisponibles = _controller.ObtenerAutosDisponibles();

            string reporte = "";
            reporte += "=".PadRight(80, '=') + "\n";
            reporte += "📊 REPORTE GENERAL DEL SISTEMA - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            reporte += $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            reporte += $"Usuario: Administrador\n\n";

            // ESTADÍSTICAS PRINCIPALES
            reporte += "RESUMEN EJECUTIVO:\n";
            reporte += "-".PadRight(50, '-') + "\n";
            reporte += $"Total de vehículos en sistema: {autos.Count}\n";
            reporte += $"Vehículos disponibles: {autosDisponibles.Count}\n";
            reporte += $"Vehículos vendidos: {autos.Count - autosDisponibles.Count}\n";
            reporte += $"Total de ventas registradas: {ventas.Count}\n";
            reporte += $"Valor total del inventario: ${_controller.ValorTotalInventario():N2}\n";
            reporte += $"Ingresos por ventas: ${ventas.Sum(v => v.PrecioFinal):N2}\n\n";

            // DISTRIBUCIÓN POR MARCAS
            reporte += "DISTRIBUCIÓN POR MARCAS:\n";
            reporte += "-".PadRight(50, '-') + "\n";
            var marcas = autosDisponibles.GroupBy(a => a.Marca)
                                        .Select(g => new { Marca = g.Key, Count = g.Count() })
                                        .OrderByDescending(x => x.Count);

            foreach (var marca in marcas)
            {
                reporte += $"{marca.Marca.PadRight(15)}: {marca.Count} vehículos\n";
            }
            reporte += "\n";

            // VEHÍCULOS MÁS NUEVOS
            reporte += "VEHÍCULOS MÁS NUEVOS (Top 5):\n";
            reporte += "-".PadRight(50, '-') + "\n";
            var nuevos = autosDisponibles.OrderByDescending(a => a.Ano).Take(5);
            foreach (var auto in nuevos)
            {
                reporte += $"{auto.Marca} {auto.Modelo} {auto.Ano} - ${auto.Precio:N2}\n";
            }
            reporte += "\n";

            // ÚLTIMAS VENTAS
            if (ventas.Any())
            {
                reporte += "ÚLTIMAS VENTAS (Top 5):\n";
                reporte += "-".PadRight(50, '-') + "\n";
                var ultimasVentas = ventas.OrderByDescending(v => v.FechaVenta).Take(5);
                foreach (var venta in ultimasVentas)
                {
                    reporte += $"{venta.FechaVenta:dd/MM/yyyy}: {venta.Cliente} - ${venta.PrecioFinal:N2}\n";
                }
            }

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE GENERAL\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        private string GenerarReporteInventario()
        {
            var autos = _controller.ObtenerAutosDisponibles().OrderBy(a => a.Marca).ThenBy(a => a.Modelo);

            string reporte = "";
            reporte += "=".PadRight(80, '=') + "\n";
            reporte += "🚗 INVENTARIO COMPLETO DE VEHÍCULOS - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            reporte += $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            reporte += $"Total de vehículos disponibles: {autos.Count()}\n";
            reporte += $"Valor total del inventario: ${_controller.ValorTotalInventario():N2}\n\n";

            reporte += "DETALLE DE VEHÍCULOS DISPONIBLES:\n";
            reporte += "-".PadRight(80, '-') + "\n";
            reporte += "CÓDIGO".PadRight(10) + " | " +
                      "MARCA".PadRight(12) + " | " +
                      "MODELO".PadRight(15) + " | " +
                      "AÑO".PadRight(6) + " | " +
                      "PRECIO".PadRight(12) + " | " +
                      "TIPO".PadRight(10) + "\n";
            reporte += "-".PadRight(80, '-') + "\n";

            foreach (var auto in autos)
            {
                reporte += $"{auto.Codigo,-10} | " +
                          $"{auto.Marca,-12} | " +
                          $"{auto.Modelo,-15} | " +
                          $"{auto.Ano,-6} | " +
                          $"${auto.Precio,10:N2} | " +
                          $"{auto.Tipo,-10}\n";
            }

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE DE INVENTARIO\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        private string GenerarReporteVentas()
        {
            var ventas = _controller.Ventas.OrderByDescending(v => v.FechaVenta);

            string reporte = "";
            reporte += "=".PadRight(80, '=') + "\n";
            reporte += "💰 REPORTE DE VENTAS DETALLADO - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            reporte += $"Fecha de generación: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            reporte += $"Total de ventas registradas: {ventas.Count()}\n";
            reporte += $"Ingresos totales: ${ventas.Sum(v => v.PrecioFinal):N2}\n";
            reporte += $"Descuentos totales aplicados: ${ventas.Sum(v => v.DescuentoAplicado):N2}\n\n";

            reporte += "DETALLE DE VENTAS:\n";
            reporte += "-".PadRight(90, '-') + "\n";
            reporte += "FECHA".PadRight(16) + " | " +
                      "CLIENTE".PadRight(20) + " | " +
                      "VENDEDOR".PadRight(15) + " | " +
                      "VEHÍCULO".PadRight(12) + " | " +
                      "PRECIO".PadRight(10) + " | " +
                      "DESCUENTO".PadRight(10) + " | " +
                      "TOTAL".PadRight(10) + "\n";
            reporte += "-".PadRight(90, '-') + "\n";

            foreach (var venta in ventas)
            {
                reporte += $"{venta.FechaVenta:dd/MM/yyyy HH:mm}".PadRight(16) + " | " +
                          $"{venta.Cliente}".PadRight(20) + " | " +
                          $"{venta.Vendedor}".PadRight(15) + " | " +
                          $"{venta.CodigoAuto}".PadRight(12) + " | " +
                          $"${venta.PrecioVenta,8:N2} | " +
                          $"${venta.DescuentoAplicado,8:N2} | " +
                          $"${venta.PrecioFinal,8:N2}\n";
            }

            // ESTADÍSTICAS DE VENTAS
            if (ventas.Any())
            {
                reporte += "\nESTADÍSTICAS DE VENTAS:\n";
                reporte += "-".PadRight(50, '-') + "\n";
                reporte += $"Venta más alta: ${ventas.Max(v => v.PrecioFinal):N2}\n";
                reporte += $"Venta promedio: ${ventas.Average(v => v.PrecioFinal):N2}\n";
                reporte += $"Venta más baja: ${ventas.Min(v => v.PrecioFinal):N2}\n";
                reporte += $"Total de descuentos: ${ventas.Sum(v => v.DescuentoAplicado):N2}\n";
            }

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE DE VENTAS\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        private string GenerarReporteFinanciero()
        {
            var autos = _controller.ObtenerAutosDisponibles();
            var ventas = _controller.Ventas;

            string reporte = "";
            reporte += "=".PadRight(80, '=') + "\n";
            reporte += "📈 REPORTE FINANCIERO - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            // ACTIVOS (INVENTARIO)
            reporte += "ACTIVOS - INVENTARIO ACTUAL:\n";
            reporte += "-".PadRight(50, '-') + "\n";
            reporte += $"Valor total del inventario: ${_controller.ValorTotalInventario():N2}\n";
            reporte += $"Cantidad de vehículos disponibles: {autos.Count()}\n";
            reporte += $"Valor promedio por vehículo: ${autos.Average(a => a.Precio):N2}\n\n";

            // INGRESOS (VENTAS)
            reporte += "INGRESOS - HISTORIAL DE VENTAS:\n";
            reporte += "-".PadRight(50, '-') + "\n";
            reporte += $"Total de ingresos por ventas: ${ventas.Sum(v => v.PrecioFinal):N2}\n";
            reporte += $"Total de descuentos otorgados: ${ventas.Sum(v => v.DescuentoAplicado):N2}\n";
            reporte += $"Ventas totales registradas: {ventas.Count}\n\n";

            // ANÁLISIS POR MARCAS
            reporte += "VALOR DE INVENTARIO POR MARCA:\n";
            reporte += "-".PadRight(50, '-') + "\n";
            var inventarioPorMarca = autos.GroupBy(a => a.Marca)
                                         .Select(g => new {
                                             Marca = g.Key,
                                             Valor = g.Sum(a => a.Precio),
                                             Cantidad = g.Count()
                                         })
                                         .OrderByDescending(x => x.Valor);

            foreach (var marca in inventarioPorMarca)
            {
                decimal porcentaje = (marca.Valor / _controller.ValorTotalInventario()) * 100;
                reporte += $"{marca.Marca.PadRight(12)}: ${marca.Valor,10:N2} ({marca.Cantidad} vehículos) - {porcentaje:N1}%\n";
            }

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE FINANCIERO\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        private string GenerarReporteConsultas()
        {
            string reporte = "";
            reporte += "=".PadRight(80, '=') + "\n";
            reporte += "🎯 REPORTE DE CONSULTAS ESPECIALES - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            // Obtener datos para las consultas
            var masAntiguo = _controller.ObtenerAutoMasAntiguo();
            var mayorCilindraje = _controller.ObtenerAutoMayorCilindraje();
            var precioMasBajo = _controller.ObtenerAutoPrecioMasBajo();

            reporte += "CONSULTA 1: VEHÍCULO MÁS ANTIGUO\n";
            reporte += "-".PadRight(50, '-') + "\n";
            if (masAntiguo != null)
            {
                reporte += $"Vehículo: {masAntiguo.Marca} {masAntiguo.Modelo}\n";
                reporte += $"Año: {masAntiguo.Ano}\n";
                reporte += $"Precio: ${masAntiguo.Precio:N2}\n";
                reporte += $"Código: {masAntiguo.Codigo}\n";
            }
            else
            {
                reporte += "No hay vehículos disponibles\n";
            }
            reporte += "\n";

            reporte += "CONSULTA 2: VEHÍCULO CON MAYOR CILINDRAJE\n";
            reporte += "-".PadRight(50, '-') + "\n";
            if (mayorCilindraje != null)
            {
                reporte += $"Vehículo: {mayorCilindraje.Marca} {mayorCilindraje.Modelo}\n";
                reporte += $"Cilindraje: {mayorCilindraje.Cilindraje} cc\n";
                reporte += $"Precio: ${mayorCilindraje.Precio:N2}\n";
                reporte += $"Código: {mayorCilindraje.Codigo}\n";
            }
            else
            {
                reporte += "No hay vehículos disponibles\n";
            }
            reporte += "\n";

            reporte += "CONSULTA 3: VEHÍCULO CON PRECIO MÁS BAJO\n";
            reporte += "-".PadRight(50, '-') + "\n";
            if (precioMasBajo != null)
            {
                reporte += $"Vehículo: {precioMasBajo.Marca} {precioMasBajo.Modelo}\n";
                reporte += $"Precio: ${precioMasBajo.Precio:N2}\n";
                reporte += $"Año: {precioMasBajo.Ano}\n";
                reporte += $"Código: {precioMasBajo.Codigo}\n";
            }
            else
            {
                reporte += "No hay vehículos disponibles\n";
            }

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE DE CONSULTAS\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        private string GenerarReporteBajaRotacion()
        {
            var autos = _controller.Autos.Where(a => a.Disponible)
                                        .OrderBy(a => a.Ano) // Los más antiguos primero
                                        .Take(5); // Top 5 más antiguos

            string reporte = "";
            reporte += "=".PadRight(80, '=') + "\n";
            reporte += "⚠️ REPORTE DE VEHÍCULOS CON BAJA ROTACIÓN - AUTOS AJH\n";
            reporte += "=".PadRight(80, '=') + "\n\n";

            reporte += "ESTOS VEHÍCULOS PODRÍAN REQUERIR ACCIONES ESPECIALES:\n";
            reporte += "(Promociones, descuentos adicionales, etc.)\n\n";

            reporte += "VEHÍCULOS IDENTIFICADOS:\n";
            reporte += "-".PadRight(70, '-') + "\n";
            reporte += "MARCA".PadRight(12) + " | " +
                      "MODELO".PadRight(15) + " | " +
                      "AÑO".PadRight(6) + " | " +
                      "PRECIO".PadRight(12) + " | " +
                      "DÍAS EN INVENTARIO".PadRight(18) + " | " +
                      "RECOMENDACIÓN\n";
            reporte += "-".PadRight(70, '-') + "\n";

            foreach (var auto in autos)
            {
                // Simular días en inventario (en un sistema real esto vendría de la base de datos)
                int diasEnInventario = (DateTime.Now.Year - auto.Ano) * 365 + new Random().Next(30, 180);
                string recomendacion = diasEnInventario > 500 ? "🔥 DESCUENTO URGENTE" :
                                      diasEnInventario > 300 ? "📉 CONSIDERAR PROMOCIÓN" : "👀 MONITOREAR";

                reporte += $"{auto.Marca,-12} | " +
                          $"{auto.Modelo,-15} | " +
                          $"{auto.Ano,-6} | " +
                          $"${auto.Precio,10:N2} | " +
                          $"{diasEnInventario,-18} | " +
                          $"{recomendacion}\n";
            }

            reporte += "\nRECOMENDACIONES GENERALES:\n";
            reporte += "-".PadRight(50, '-') + "\n";
            reporte += "• Revisar precios de vehículos con más de 1 año en inventario\n";
            reporte += "• Considerar campañas de marketing específicas\n";
            reporte += "• Evaluar descuentos progresivos por tiempo en inventario\n";
            reporte += "• Rotar exhibición de vehículos en showroom\n";

            reporte += "\n" + "=".PadRight(80, '=') + "\n";
            reporte += "FIN DEL REPORTE DE BAJA ROTACIÓN\n";
            reporte += "=".PadRight(80, '=') + "\n";

            return reporte;
        }

        #endregion
    }
}