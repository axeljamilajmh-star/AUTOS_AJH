using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class FrmDescuento : Form
    {
        private readonly AutoController _controller;
        private Panel panelPrincipal;
        private Panel panelHeader;
        private Panel panelFormulario;
        private Panel panelBotones;

        // Controles del formulario
        private ComboBox cmbAutosDisponibles;
        private NumericUpDown numPorcentajeDescuento;
        private Label lblInfoAuto, lblPrecioActual, lblNuevoPrecio, lblAhorro;
        private Button btnAplicarDescuento, btnCancelar, btnBuscarAuto;
        private Auto autoSeleccionado;

        public FrmDescuento(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Aplicar Descuento - AUTOS AJH";
            this.Size = new Size(650, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            CrearInterfaz();
            CargarAutosDisponibles();
        }

        private void CrearInterfaz()
        {
            panelPrincipal = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            CrearHeader();
            CrearFormulario();
            CrearPanelBotones();

            panelPrincipal.Controls.Add(panelFormulario);
            panelPrincipal.Controls.Add(panelBotones);
            panelPrincipal.Controls.Add(panelHeader);

            this.Controls.Add(panelPrincipal);
        }

        private void CrearHeader()
        {
            panelHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(255, 193, 7), // Amarillo para descuentos
                Padding = new Padding(20, 0, 20, 0)
            };

            Label lblTitulo = new Label
            {
                Text = "🎯 APLICAR DESCUENTO",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Height = panelHeader.Height
            };

            panelHeader.Controls.Add(lblTitulo);
        }

        private void CrearFormulario()
        {
            panelFormulario = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                AutoScroll = true
            };

            CrearControlesFormulario();
        }

        private void CrearControlesFormulario()
        {
            int yPos = 20;

            // === SECCIÓN 1: SELECCIÓN DE VEHÍCULO ===
            Label lblSeccion1 = new Label
            {
                Text = "🚗 SELECCIÓN DEL VEHÍCULO",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(400, 25)
            };
            panelFormulario.Controls.Add(lblSeccion1);
            yPos += 35;

            // Selección de auto
            Label lblAuto = new Label
            {
                Text = "Vehículo para descuento (*):",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(250, 20)
            };
            panelFormulario.Controls.Add(lblAuto);

            cmbAutosDisponibles = new ComboBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(400, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f)
            };
            cmbAutosDisponibles.SelectedIndexChanged += CmbAutosDisponibles_SelectedIndexChanged;
            panelFormulario.Controls.Add(cmbAutosDisponibles);

            btnBuscarAuto = new Button
            {
                Text = "🔍 Buscar",
                Location = new Point(430, yPos + 25),
                Size = new Size(80, 25),
                BackColor = Color.FromArgb(0, 123, 255),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnBuscarAuto.Click += BtnBuscarAuto_Click;
            panelFormulario.Controls.Add(btnBuscarAuto);

            yPos += 60;

            // Información del auto seleccionado
            lblInfoAuto = new Label
            {
                Text = "Seleccione un vehículo para aplicar descuento...",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(20, yPos),
                Size = new Size(580, 50),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                BackColor = Color.FromArgb(248, 249, 250)
            };
            panelFormulario.Controls.Add(lblInfoAuto);

            yPos += 70;

            // === SECCIÓN 2: INFORMACIÓN DE PRECIOS ===
            Label lblSeccion2 = new Label
            {
                Text = "💰 INFORMACIÓN DE PRECIOS",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(400, 25)
            };
            panelFormulario.Controls.Add(lblSeccion2);
            yPos += 35;

            // Precio actual
            Label lblPrecioActualTitulo = new Label
            {
                Text = "Precio Actual del Vehículo:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(200, 20)
            };
            panelFormulario.Controls.Add(lblPrecioActualTitulo);

            lblPrecioActual = new Label
            {
                Text = "$0.00",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 37, 41),
                Location = new Point(230, yPos),
                Size = new Size(150, 20)
            };
            panelFormulario.Controls.Add(lblPrecioActual);

            yPos += 35;

            // === SECCIÓN 3: CONFIGURACIÓN DEL DESCUENTO ===
            Label lblSeccion3 = new Label
            {
                Text = "📊 CONFIGURACIÓN DEL DESCUENTO",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(400, 25)
            };
            panelFormulario.Controls.Add(lblSeccion3);
            yPos += 35;

            // Porcentaje de descuento
            Label lblPorcentaje = new Label
            {
                Text = "Porcentaje de Descuento (%):",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(200, 20)
            };
            panelFormulario.Controls.Add(lblPorcentaje);

            numPorcentajeDescuento = new NumericUpDown
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(120, 25),
                Minimum = 0,
                Maximum = 10, // Máximo 10% según requerimientos
                Value = 5, // Valor por defecto 5%
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69)
            };
            numPorcentajeDescuento.ValueChanged += NumPorcentajeDescuento_ValueChanged;
            panelFormulario.Controls.Add(numPorcentajeDescuento);

            Label lblPorcentajeMax = new Label
            {
                Text = "Máximo 10% permitido",
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(150, yPos + 28),
                Size = new Size(150, 20)
            };
            panelFormulario.Controls.Add(lblPorcentajeMax);

            yPos += 60;

            // === SECCIÓN 4: RESULTADO DEL DESCUENTO ===
            Label lblSeccion4 = new Label
            {
                Text = "🎯 RESULTADO DEL DESCUENTO",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(400, 25)
            };
            panelFormulario.Controls.Add(lblSeccion4);
            yPos += 35;

            // Panel de resultados
            Panel panelResultados = new Panel
            {
                Location = new Point(20, yPos),
                Size = new Size(580, 120),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(15)
            };

            // Ahorro
            Label lblAhorroTitulo = new Label
            {
                Text = "Descuento Aplicado:",
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Location = new Point(10, 15),
                Size = new Size(200, 25)
            };
            panelResultados.Controls.Add(lblAhorroTitulo);

            lblAhorro = new Label
            {
                Text = "$0.00",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(220, 53, 69),
                Location = new Point(220, 15),
                Size = new Size(150, 25)
            };
            panelResultados.Controls.Add(lblAhorro);

            // Nuevo precio
            Label lblNuevoPrecioTitulo = new Label
            {
                Text = "NUEVO PRECIO:",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Location = new Point(10, 50),
                Size = new Size(200, 30)
            };
            panelResultados.Controls.Add(lblNuevoPrecioTitulo);

            lblNuevoPrecio = new Label
            {
                Text = "$0.00",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 167, 69),
                Location = new Point(220, 50),
                Size = new Size(200, 30)
            };
            panelResultados.Controls.Add(lblNuevoPrecio);

            // Porcentaje aplicado
            Label lblPorcentajeAplicado = new Label
            {
                Text = "Porcentaje aplicado: 0%",
                Name = "lblPorcentajeAplicado",
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(10, 85),
                Size = new Size(300, 20)
            };
            panelResultados.Controls.Add(lblPorcentajeAplicado);

            panelFormulario.Controls.Add(panelResultados);
        }

        private void CrearPanelBotones()
        {
            panelBotones = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 80,
                BackColor = Color.FromArgb(248, 249, 250),
                Padding = new Padding(20)
            };

            // Botón Aplicar Descuento
            btnAplicarDescuento = new Button
            {
                Text = "🎯 APLICAR DESCUENTO",
                Size = new Size(180, 45),
                Location = new Point(350, 15),
                BackColor = Color.FromArgb(255, 193, 7),
                ForeColor = Color.FromArgb(33, 37, 41),
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnAplicarDescuento.Click += BtnAplicarDescuento_Click;

            // Botón Cancelar
            btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Size = new Size(120, 45),
                Location = new Point(220, 15),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => this.Close();

            panelBotones.Controls.AddRange(new Control[] { btnAplicarDescuento, btnCancelar });
        }

        private void CargarAutosDisponibles()
        {
            cmbAutosDisponibles.Items.Clear();

            var autosDisponibles = _controller.ObtenerAutosDisponibles();

            foreach (var auto in autosDisponibles)
            {
                cmbAutosDisponibles.Items.Add(new AutoComboBoxItem(auto));
            }

            if (cmbAutosDisponibles.Items.Count > 0)
            {
                cmbAutosDisponibles.SelectedIndex = 0;
            }
            else
            {
                lblInfoAuto.Text = "❌ No hay vehículos disponibles para aplicar descuentos.";
                btnAplicarDescuento.Enabled = false;
                numPorcentajeDescuento.Enabled = false;
            }
        }

        // Clase auxiliar para mostrar autos en el ComboBox
        private class AutoComboBoxItem
        {
            public Auto Auto { get; set; }

            public AutoComboBoxItem(Auto auto)
            {
                Auto = auto;
            }

            public override string ToString()
            {
                return $"{Auto.Codigo} - {Auto.Marca} {Auto.Modelo} {Auto.Ano} - ${Auto.Precio:N2}";
            }
        }

        private void CmbAutosDisponibles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAutosDisponibles.SelectedItem is AutoComboBoxItem item)
            {
                autoSeleccionado = item.Auto;
                ActualizarInformacionAuto();
                CalcularDescuento();
            }
        }

        private void BtnBuscarAuto_Click(object sender, EventArgs e)
        {
            var buscarForm = new FrmBuscarAuto(_controller);
            if (buscarForm.ShowDialog() == DialogResult.OK)
            {
                // Recargar la lista después de buscar
                CargarAutosDisponibles();
            }
        }

        private void NumPorcentajeDescuento_ValueChanged(object sender, EventArgs e)
        {
            CalcularDescuento();
        }

        private void ActualizarInformacionAuto()
        {
            if (autoSeleccionado != null)
            {
                lblInfoAuto.Text = $"🚗 {autoSeleccionado.Marca} {autoSeleccionado.Modelo} {autoSeleccionado.Ano}\n" +
                                 $"📋 Código: {autoSeleccionado.Codigo} | 🎨 Color: {autoSeleccionado.Color}\n" +
                                 $"⚙️ Tipo: {autoSeleccionado.Tipo} | 🔧 {autoSeleccionado.Transmision}";

                lblPrecioActual.Text = $"${autoSeleccionado.Precio:N2}";
            }
        }

        private void CalcularDescuento()
        {
            if (autoSeleccionado != null)
            {
                decimal precioActual = autoSeleccionado.Precio;
                decimal porcentajeDescuento = numPorcentajeDescuento.Value;
                decimal descuentoDinero = precioActual * (porcentajeDescuento / 100);
                decimal nuevoPrecio = precioActual - descuentoDinero;

                lblAhorro.Text = $"${descuentoDinero:N2}";
                lblNuevoPrecio.Text = $"${nuevoPrecio:N2}";

                // Actualizar etiqueta de porcentaje aplicado
                var lblPorcentajeAplicado = panelFormulario.Controls.Find("lblPorcentajeAplicado", true).FirstOrDefault() as Label;
                if (lblPorcentajeAplicado != null)
                {
                    lblPorcentajeAplicado.Text = $"Porcentaje aplicado: {porcentajeDescuento}%";
                }
            }
        }

        private void BtnAplicarDescuento_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (!ValidarCamposObligatorios())
                    return;

                if (autoSeleccionado == null)
                {
                    MessageBox.Show("❌ Debe seleccionar un vehículo para aplicar el descuento.",
                                  "Validación",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                // Obtener datos del descuento
                decimal porcentajeDescuento = numPorcentajeDescuento.Value;
                decimal precioOriginal = autoSeleccionado.Precio;
                decimal descuentoDinero = precioOriginal * (porcentajeDescuento / 100);
                decimal nuevoPrecio = precioOriginal - descuentoDinero;

                // Mostrar confirmación MÁS CLARA
                string mensajeConfirmacion = $"¿Está seguro de aplicar {porcentajeDescuento}% de descuento a este vehículo?\n\n" +
                                           $"🚗 VEHÍCULO: {autoSeleccionado.Marca} {autoSeleccionado.Modelo} {autoSeleccionado.Ano}\n" +
                                           $"📋 CÓDIGO: {autoSeleccionado.Codigo}\n\n" +
                                           $"💰 PRECIO ACTUAL: ${precioOriginal:N2}\n" +
                                           $"🎯 DESCUENTO: {porcentajeDescuento}% (${descuentoDinero:N2})\n" +
                                           $"💰 NUEVO PRECIO: ${nuevoPrecio:N2}\n\n" +
                                           $"📝 **IMPORTANTE:**\n" +
                                           $"• El vehículo seguirá DISPONIBLE para la venta\n" +
                                           $"• El nuevo precio será el que aparezca en el catálogo\n" +
                                           $"• Los clientes verán el precio con descuento directamente";

                DialogResult resultado = MessageBox.Show(mensajeConfirmacion,
                                                       "🎯 CONFIRMAR APLICACIÓN DE DESCUENTO",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Aplicar el descuento (SOLO cambia el precio, NO vende el auto)
                    bool descuentoExitoso = _controller.AplicarDescuento(
                        autoSeleccionado.Codigo,
                        porcentajeDescuento
                    );

                    if (descuentoExitoso)
                    {
                        MessageBox.Show($"✅ ¡DESCUENTO APLICADO EXITOSAMENTE!\n\n" +
                                      $"🚗 Vehículo: {autoSeleccionado.Marca} {autoSeleccionado.Modelo}\n" +
                                      $"📋 Código: {autoSeleccionado.Codigo}\n\n" +
                                      $"💰 Precio Original: ${precioOriginal:N2}\n" +
                                      $"🎯 Descuento Aplicado: {porcentajeDescuento}%\n" +
                                      $"💵 Ahorro: ${descuentoDinero:N2}\n" +
                                      $"💰 NUEVO PRECIO: ${nuevoPrecio:N2}\n\n" +
                                      $"📢 El vehículo sigue DISPONIBLE en el catálogo con el nuevo precio.",
                                      "🎯 DESCUENTO APLICADO",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        // Cerrar el formulario
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al aplicar el descuento:\n{ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private bool ValidarCamposObligatorios()
        {
            if (cmbAutosDisponibles.SelectedItem == null)
            {
                MostrarError("Debe seleccionar un vehículo para aplicar el descuento.", cmbAutosDisponibles);
                return false;
            }

            if (numPorcentajeDescuento.Value <= 0)
            {
                MostrarError("El porcentaje de descuento debe ser mayor a 0%.", numPorcentajeDescuento);
                return false;
            }

            return true;
        }

        private void MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }
    }
}