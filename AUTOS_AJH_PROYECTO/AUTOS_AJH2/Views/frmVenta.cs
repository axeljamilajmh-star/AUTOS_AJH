using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AUTOS_AJH.Views
{
    public class frmVenta : Form
    {
        private AutoController _controller;
        private Panel panelPrincipal;
        private Panel panelHeader;
        private Panel panelFormulario;
        private Panel panelBotones;

        // Controles del formulario
        private ComboBox cmbAutosDisponibles;
        private TextBox txtCliente, txtVendedor;
        private NumericUpDown numDescuento;
        private Label lblInfoAuto, lblPrecioBase, lblDescuentoAplicado, lblPrecioFinal;
        private Button btnRegistrarVenta, btnCancelar, btnBuscarAuto;
        private Auto autoSeleccionado;

        public frmVenta(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Registrar Venta - AUTOS AJH";
            this.Size = new Size(700, 550);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;
            this.Padding = new Padding(10);

            CrearInterfaz();
            CargarAutosDisponibles();
        }

        private void CrearInterfaz()
        {
            panelPrincipal = new Panel();
            panelPrincipal.Dock = DockStyle.Fill;
            panelPrincipal.BackColor = Color.White;

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
            panelHeader = new Panel();
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Height = 80;
            panelHeader.BackColor = Color.FromArgb(40, 167, 69); // Verde para ventas
            panelHeader.Padding = new Padding(20, 0, 20, 0);

            Label lblTitulo = new Label();
            lblTitulo.Text = "💰 REGISTRAR VENTA";
            lblTitulo.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Dock = DockStyle.Left;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            lblTitulo.Height = panelHeader.Height;

            panelHeader.Controls.Add(lblTitulo);
        }

        private void CrearFormulario()
        {
            panelFormulario = new Panel();
            panelFormulario.Dock = DockStyle.Fill;
            panelFormulario.Padding = new Padding(30);
            panelFormulario.AutoScroll = true;

            CrearControlesFormulario();
        }

        private void CrearControlesFormulario()
        {
            int yPos = 20;

            // === SECCIÓN 1: SELECCIÓN DE VEHÍCULO ===
            Label lblSeccion1 = new Label();
            lblSeccion1.Text = "🚗 SELECCIÓN DEL VEHÍCULO";
            lblSeccion1.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblSeccion1.ForeColor = Color.FromArgb(0, 122, 204);
            lblSeccion1.Location = new Point(20, yPos);
            lblSeccion1.Size = new Size(400, 25);
            panelFormulario.Controls.Add(lblSeccion1);
            yPos += 35;

            // Selección de auto
            Label lblAuto = new Label();
            lblAuto.Text = "Vehículo a vender (*):";
            lblAuto.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblAuto.Location = new Point(20, yPos);
            lblAuto.Size = new Size(200, 20);
            panelFormulario.Controls.Add(lblAuto);

            cmbAutosDisponibles = new ComboBox();
            cmbAutosDisponibles.Location = new Point(20, yPos + 25);
            cmbAutosDisponibles.Size = new Size(400, 25);
            cmbAutosDisponibles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbAutosDisponibles.Font = new Font("Segoe UI", 9f);
            cmbAutosDisponibles.SelectedIndexChanged += CmbAutosDisponibles_SelectedIndexChanged;
            panelFormulario.Controls.Add(cmbAutosDisponibles);

            btnBuscarAuto = new Button();
            btnBuscarAuto.Text = "🔍 Buscar";
            btnBuscarAuto.Location = new Point(430, yPos + 25);
            btnBuscarAuto.Size = new Size(80, 25);
            btnBuscarAuto.BackColor = Color.FromArgb(0, 123, 255);
            btnBuscarAuto.ForeColor = Color.White;
            btnBuscarAuto.FlatStyle = FlatStyle.Flat;
            btnBuscarAuto.Click += BtnBuscarAuto_Click;
            panelFormulario.Controls.Add(btnBuscarAuto);

            yPos += 60;

            // Información del auto seleccionado
            lblInfoAuto = new Label();
            lblInfoAuto.Text = "Seleccione un vehículo para ver su información...";
            lblInfoAuto.Font = new Font("Segoe UI", 9f, FontStyle.Italic);
            lblInfoAuto.ForeColor = Color.FromArgb(108, 117, 125);
            lblInfoAuto.Location = new Point(20, yPos);
            lblInfoAuto.Size = new Size(600, 40);
            lblInfoAuto.BorderStyle = BorderStyle.FixedSingle;
            lblInfoAuto.Padding = new Padding(10);
            lblInfoAuto.BackColor = Color.FromArgb(248, 249, 250);
            panelFormulario.Controls.Add(lblInfoAuto);

            yPos += 60;

            // === SECCIÓN 2: INFORMACIÓN DE LA VENTA ===
            Label lblSeccion2 = new Label();
            lblSeccion2.Text = "👥 INFORMACIÓN DE LA VENTA";
            lblSeccion2.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblSeccion2.ForeColor = Color.FromArgb(0, 122, 204);
            lblSeccion2.Location = new Point(20, yPos);
            lblSeccion2.Size = new Size(400, 25);
            panelFormulario.Controls.Add(lblSeccion2);
            yPos += 35;

            // Cliente
            Label lblCliente = new Label();
            lblCliente.Text = "Nombre del Cliente (*):";
            lblCliente.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblCliente.Location = new Point(20, yPos);
            lblCliente.Size = new Size(200, 20);
            panelFormulario.Controls.Add(lblCliente);

            txtCliente = new TextBox();
            txtCliente.Location = new Point(20, yPos + 25);
            txtCliente.Size = new Size(300, 25);
            txtCliente.Font = new Font("Segoe UI", 9f);
            panelFormulario.Controls.Add(txtCliente);

            yPos += 60;

            // Vendedor
            Label lblVendedor = new Label();
            lblVendedor.Text = "Nombre del Vendedor (*):";
            lblVendedor.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblVendedor.Location = new Point(20, yPos);
            lblVendedor.Size = new Size(200, 20);
            panelFormulario.Controls.Add(lblVendedor);

            txtVendedor = new TextBox();
            txtVendedor.Location = new Point(20, yPos + 25);
            txtVendedor.Size = new Size(300, 25);
            txtVendedor.Font = new Font("Segoe UI", 9f);
            panelFormulario.Controls.Add(txtVendedor);

            yPos += 60;

            // === SECCIÓN 3: DETALLES DE PAGO ===
            Label lblSeccion3 = new Label();
            lblSeccion3.Text = "💳 DETALLES DE PAGO";
            lblSeccion3.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            lblSeccion3.ForeColor = Color.FromArgb(0, 122, 204);
            lblSeccion3.Location = new Point(20, yPos);
            lblSeccion3.Size = new Size(400, 25);
            panelFormulario.Controls.Add(lblSeccion3);
            yPos += 35;

            // Precio base
            Label lblPrecioBaseTitulo = new Label();
            lblPrecioBaseTitulo.Text = "Precio Base del Vehículo:";
            lblPrecioBaseTitulo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblPrecioBaseTitulo.Location = new Point(20, yPos);
            lblPrecioBaseTitulo.Size = new Size(200, 20);
            panelFormulario.Controls.Add(lblPrecioBaseTitulo);

            lblPrecioBase = new Label();
            lblPrecioBase.Text = "$0.00";
            lblPrecioBase.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblPrecioBase.ForeColor = Color.FromArgb(40, 167, 69);
            lblPrecioBase.Location = new Point(230, yPos);
            lblPrecioBase.Size = new Size(150, 20);
            panelFormulario.Controls.Add(lblPrecioBase);

            yPos += 30;

            // Descuento
            Label lblDescuento = new Label();
            lblDescuento.Text = "Descuento Aplicado (%):";
            lblDescuento.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblDescuento.Location = new Point(20, yPos);
            lblDescuento.Size = new Size(200, 20);
            panelFormulario.Controls.Add(lblDescuento);

            numDescuento = new NumericUpDown();
            numDescuento.Location = new Point(20, yPos + 25);
            numDescuento.Size = new Size(100, 25);
            numDescuento.Minimum = 0;
            numDescuento.Maximum = 10; // Máximo 10% de descuento
            numDescuento.Value = 0;
            numDescuento.Font = new Font("Segoe UI", 9f);
            numDescuento.ValueChanged += NumDescuento_ValueChanged;
            panelFormulario.Controls.Add(numDescuento);

            Label lblDescuentoMax = new Label();
            lblDescuentoMax.Text = "(Máximo 10%)";
            lblDescuentoMax.Font = new Font("Segoe UI", 8f, FontStyle.Italic);
            lblDescuentoMax.ForeColor = Color.FromArgb(108, 117, 125);
            lblDescuentoMax.Location = new Point(125, yPos + 28);
            lblDescuentoMax.Size = new Size(100, 20);
            panelFormulario.Controls.Add(lblDescuentoMax);

            yPos += 30;

            // Descuento aplicado en dinero
            Label lblDescuentoTitulo = new Label();
            lblDescuentoTitulo.Text = "Descuento en Dinero:";
            lblDescuentoTitulo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblDescuentoTitulo.Location = new Point(20, yPos);
            lblDescuentoTitulo.Size = new Size(200, 20);
            panelFormulario.Controls.Add(lblDescuentoTitulo);

            lblDescuentoAplicado = new Label();
            lblDescuentoAplicado.Text = "$0.00";
            lblDescuentoAplicado.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblDescuentoAplicado.ForeColor = Color.FromArgb(220, 53, 69);
            lblDescuentoAplicado.Location = new Point(230, yPos);
            lblDescuentoAplicado.Size = new Size(150, 20);
            panelFormulario.Controls.Add(lblDescuentoAplicado);

            yPos += 30;

            // Precio final
            Label lblPrecioFinalTitulo = new Label();
            lblPrecioFinalTitulo.Text = "PRECIO FINAL:";
            lblPrecioFinalTitulo.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            lblPrecioFinalTitulo.Location = new Point(20, yPos);
            lblPrecioFinalTitulo.Size = new Size(200, 25);
            panelFormulario.Controls.Add(lblPrecioFinalTitulo);

            lblPrecioFinal = new Label();
            lblPrecioFinal.Text = "$0.00";
            lblPrecioFinal.Font = new Font("Segoe UI", 14f, FontStyle.Bold);
            lblPrecioFinal.ForeColor = Color.FromArgb(40, 167, 69);
            lblPrecioFinal.Location = new Point(230, yPos);
            lblPrecioFinal.Size = new Size(200, 25);
            panelFormulario.Controls.Add(lblPrecioFinal);

            yPos += 40;
        }

        private void CrearPanelBotones()
        {
            panelBotones = new Panel();
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Height = 80;
            panelBotones.BackColor = Color.FromArgb(248, 249, 250);
            panelBotones.Padding = new Padding(20);

            // Botón Registrar Venta
            btnRegistrarVenta = new Button();
            btnRegistrarVenta.Text = "💰 REGISTRAR VENTA";
            btnRegistrarVenta.Size = new Size(180, 45);
            btnRegistrarVenta.Location = new Point(350, 15);
            btnRegistrarVenta.BackColor = Color.FromArgb(40, 167, 69);
            btnRegistrarVenta.ForeColor = Color.White;
            btnRegistrarVenta.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnRegistrarVenta.FlatStyle = FlatStyle.Flat;
            btnRegistrarVenta.Click += BtnRegistrarVenta_Click;

            // Botón Cancelar
            btnCancelar = new Button();
            btnCancelar.Text = "❌ CANCELAR";
            btnCancelar.Size = new Size(120, 45);
            btnCancelar.Location = new Point(220, 15);
            btnCancelar.BackColor = Color.FromArgb(220, 53, 69);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Click += (s, e) => this.Close();

            panelBotones.Controls.AddRange(new Control[] { btnRegistrarVenta, btnCancelar });
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
                lblInfoAuto.Text = "❌ No hay vehículos disponibles para la venta.";
                btnRegistrarVenta.Enabled = false;
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
                CalcularPrecioFinal();
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

        private void NumDescuento_ValueChanged(object sender, EventArgs e)
        {
            CalcularPrecioFinal();
        }

        private void ActualizarInformacionAuto()
        {
            if (autoSeleccionado != null)
            {
                lblInfoAuto.Text = $"🚗 {autoSeleccionado.Marca} {autoSeleccionado.Modelo} {autoSeleccionado.Ano}\n" +
                                 $"📋 Código: {autoSeleccionado.Codigo} | 🎨 Color: {autoSeleccionado.Color}\n" +
                                 $"⚙️ Tipo: {autoSeleccionado.Tipo} | 🔧 {autoSeleccionado.Transmision}";

                lblPrecioBase.Text = $"${autoSeleccionado.Precio:N2}";
            }
        }

        private void CalcularPrecioFinal()
        {
            if (autoSeleccionado != null)
            {
                decimal precioBase = autoSeleccionado.Precio;
                decimal porcentajeDescuento = numDescuento.Value;
                decimal descuentoDinero = precioBase * (porcentajeDescuento / 100);
                decimal precioFinal = precioBase - descuentoDinero;

                lblDescuentoAplicado.Text = $"${descuentoDinero:N2}";
                lblPrecioFinal.Text = $"${precioFinal:N2}";
            }
        }

        private void BtnRegistrarVenta_Click(object sender, EventArgs e)
        {
            try
            {
                // Validaciones
                if (!ValidarCamposObligatorios())
                    return;

                if (autoSeleccionado == null)
                {
                    MessageBox.Show("❌ Debe seleccionar un vehículo para la venta.",
                                  "Validación",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Warning);
                    return;
                }

                // Confirmar la venta
                decimal descuento = autoSeleccionado.Precio * (numDescuento.Value / 100);
                decimal precioFinal = autoSeleccionado.Precio - descuento;

                string mensajeConfirmacion = $"¿Está seguro de registrar la venta?\n\n" +
                                           $"🚗 VEHÍCULO: {autoSeleccionado.Marca} {autoSeleccionado.Modelo}\n" +
                                           $"📋 CÓDIGO: {autoSeleccionado.Codigo}\n" +
                                           $"👤 CLIENTE: {txtCliente.Text}\n" +
                                           $"👨‍💼 VENDEDOR: {txtVendedor.Text}\n" +
                                           $"💰 PRECIO FINAL: ${precioFinal:N2}\n\n" +
                                           $"⚠️ Esta acción no se puede deshacer.";

                DialogResult resultado = MessageBox.Show(mensajeConfirmacion,
                                                       "Confirmar Venta",
                                                       MessageBoxButtons.YesNo,
                                                       MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    // Registrar la venta
                    bool ventaExitosa = _controller.RegistrarVenta(
                        autoSeleccionado.Codigo,
                        descuento,
                        txtVendedor.Text.Trim(),
                        txtCliente.Text.Trim()
                    );

                    if (ventaExitosa)
                    {
                        MessageBox.Show($"✅ Venta registrada exitosamente!\n\n" +
                                      $"Vehículo: {autoSeleccionado.Marca} {autoSeleccionado.Modelo}\n" +
                                      $"Cliente: {txtCliente.Text}\n" +
                                      $"Precio Final: ${precioFinal:N2}\n" +
                                      $"Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}",
                                      "Venta Exitosa",
                                      MessageBoxButtons.OK,
                                      MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al registrar la venta:\n{ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }

        private bool ValidarCamposObligatorios()
        {
            if (string.IsNullOrWhiteSpace(txtCliente.Text))
            {
                MostrarError("El nombre del cliente es obligatorio.", txtCliente);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtVendedor.Text))
            {
                MostrarError("El nombre del vendedor es obligatorio.", txtVendedor);
                return false;
            }

            if (cmbAutosDisponibles.SelectedItem == null)
            {
                MostrarError("Debe seleccionar un vehículo para la venta.", cmbAutosDisponibles);
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