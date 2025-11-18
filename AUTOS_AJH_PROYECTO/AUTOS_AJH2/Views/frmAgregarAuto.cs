using AUTOS_AJH.Controllers;
using AUTOS_AJH.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AUTOS_AJH2.Models;


namespace AUTOS_AJH.Views
{
    public class FrmAgregarAuto : Form
    {
        DB_AUTO bd = new DB_AUTO();

        private readonly AutoController _controller;
        private Panel panelPrincipal;
        private Panel panelHeader;
        private Panel panelFormulario;
        private Panel panelBotones;

        // Controles del formulario
        private TextBox txtCodigo, txtMarca, txtModelo, txtColor, txtDescripcion;
        private NumericUpDown numAno, numPrecio, numCilindraje, numPuertas, numPasajeros;
        private ComboBox cmbTipo, cmbTransmision, cmbCombustible;
        private Button btnGuardar, btnCancelar, btnLimpiar;
        private Label lblMensaje;

        public FrmAgregarAuto(AutoController controller)
        {
            _controller = controller;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Agregar Nuevo Vehículo - AUTOS AJH";
            this.Size = new Size(800, 700);
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
                BackColor = Color.FromArgb(0, 122, 204),
                Padding = new Padding(20, 0, 20, 0)
            };

            Label lblTitulo = new Label
            {
                Text = "🚗 AGREGAR NUEVO VEHÍCULO",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
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
                Padding = new Padding(20),
                AutoScroll = true
            };

            // Crear controles del formulario
            CrearControlesFormulario();
        }

        private void CrearControlesFormulario()
        {
            int yPos = 20;

            // === SECCIÓN 1: INFORMACIÓN BÁSICA ===
            Label lblSeccion1 = new Label
            {
                Text = "📋 INFORMACIÓN BÁSICA",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(300, 25)
            };
            panelFormulario.Controls.Add(lblSeccion1);
            yPos += 35;

            // Fila 1: Código y Marca
            CrearCampo("Código (*):", "txtCodigo", 20, yPos, 350, out txtCodigo);
            CrearCampo("Marca (*):", "txtMarca", 400, yPos, 350, out txtMarca);
            yPos += 45;

            // Fila 2: Modelo y Año
            CrearCampo("Modelo (*):", "txtModelo", 20, yPos, 350, out txtModelo);
            CrearCampoNumerico("Año (*):", "numAno", 400, yPos, 350, out numAno, 1900, DateTime.Now.Year + 1, DateTime.Now.Year);
            yPos += 45;

            // Fila 3: Color y Tipo
            CrearCampo("Color:", "txtColor", 20, yPos, 350, out txtColor);
            CrearComboBox("Tipo:", "cmbTipo", 400, yPos, 350, out cmbTipo,
                new string[] { "Sedán", "SUV", "Hatchback", "Pickup", "Deportivo", "Van", "Coupe", "Convertible" });
            yPos += 45;

            // === SECCIÓN 2: ESPECIFICACIONES TÉCNICAS ===
            yPos += 20;
            Label lblSeccion2 = new Label
            {
                Text = "⚙️ ESPECIFICACIONES TÉCNICAS",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(300, 25)
            };
            panelFormulario.Controls.Add(lblSeccion2);
            yPos += 35;

            // Fila 4: Precio y Cilindraje
            CrearCampoNumerico("Precio ($) (*):", "numPrecio", 20, yPos, 350, out numPrecio, 0, 1000000, 25000);
            CrearCampoNumerico("Cilindraje (cc):", "numCilindraje", 400, yPos, 350, out numCilindraje, 0, 10000, 1800);
            yPos += 45;

            // Fila 5: Transmisión y Combustible
            CrearComboBox("Transmisión:", "cmbTransmision", 20, yPos, 350, out cmbTransmision,
                new string[] { "Manual", "Automática", "CVT", "Semi-automática" });
            CrearComboBox("Combustible:", "cmbCombustible", 400, yPos, 350, out cmbCombustible,
                new string[] { "Gasolina", "Diésel", "Híbrido", "Eléctrico", "Gas" });
            yPos += 45;

            // Fila 6: Puertas y Pasajeros
            CrearCampoNumerico("N° Puertas:", "numPuertas", 20, yPos, 350, out numPuertas, 2, 6, 4);
            CrearCampoNumerico("N° Pasajeros:", "numPasajeros", 400, yPos, 350, out numPasajeros, 1, 9, 5);
            yPos += 45;

            // === SECCIÓN 3: DESCRIPCIÓN ===
            yPos += 20;
            Label lblSeccion3 = new Label
            {
                Text = "📝 DESCRIPCIÓN ADICIONAL",
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204),
                Location = new Point(20, yPos),
                Size = new Size(300, 25)
            };
            panelFormulario.Controls.Add(lblSeccion3);
            yPos += 35;

            // Descripción
            Label lblDescripcion = new Label
            {
                Text = "Descripción:",
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(20, yPos),
                Size = new Size(150, 20)
            };
            panelFormulario.Controls.Add(lblDescripcion);

            txtDescripcion = new TextBox
            {
                Location = new Point(20, yPos + 25),
                Size = new Size(730, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Segoe UI", 9f)
            };
            panelFormulario.Controls.Add(txtDescripcion);
            yPos += 150;

            // Mensaje de validación
            lblMensaje = new Label
            {
                Text = "(*) Campos obligatorios",
                Font = new Font("Segoe UI", 8f, FontStyle.Italic),
                ForeColor = Color.FromArgb(108, 117, 125),
                Location = new Point(20, yPos),
                Size = new Size(300, 20)
            };
            panelFormulario.Controls.Add(lblMensaje);
        }

        private void CrearCampo(string etiqueta, string nombre, int x, int y, int ancho, out TextBox textBox)
        {
            Label label = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(x, y),
                Size = new Size(ancho, 20)
            };
            panelFormulario.Controls.Add(label);

            textBox = new TextBox
            {
                Name = nombre,
                Location = new Point(x, y + 25),
                Size = new Size(ancho, 23),
                Font = new Font("Segoe UI", 9f)
            };
            panelFormulario.Controls.Add(textBox);
        }

        private void CrearCampoNumerico(string etiqueta, string nombre, int x, int y, int ancho, out NumericUpDown numericUpDown, decimal min, decimal max, decimal valor)
        {
            Label label = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(x, y),
                Size = new Size(ancho, 20)
            };
            panelFormulario.Controls.Add(label);

            numericUpDown = new NumericUpDown
            {
                Name = nombre,
                Location = new Point(x, y + 25),
                Size = new Size(ancho, 23),
                Minimum = min,
                Maximum = max,
                Value = valor,
                Font = new Font("Segoe UI", 9f),
                ThousandsSeparator = true
            };
            panelFormulario.Controls.Add(numericUpDown);
        }

        private void CrearComboBox(string etiqueta, string nombre, int x, int y, int ancho, out ComboBox comboBox, string[] items)
        {
            Label label = new Label
            {
                Text = etiqueta,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Location = new Point(x, y),
                Size = new Size(ancho, 20)
            };
            panelFormulario.Controls.Add(label);

            comboBox = new ComboBox
            {
                Name = nombre,
                Location = new Point(x, y + 25),
                Size = new Size(ancho, 23),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9f)
            };
            comboBox.Items.AddRange(items);
            if (comboBox.Items.Count > 0)
                comboBox.SelectedIndex = 0;
            panelFormulario.Controls.Add(comboBox);
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

            // Botón Guardar
            btnGuardar = new Button
            {
                Text = "💾 GUARDAR VEHÍCULO",
                Size = new Size(150, 40),
                Location = new Point(450, 20),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnGuardar.Click += BtnGuardar_Click;

            // Botón Limpiar
            btnLimpiar = new Button
            {
                Text = "🔄 LIMPIAR",
                Size = new Size(120, 40),
                Location = new Point(320, 20),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnLimpiar.Click += BtnLimpiar_Click;

            // Botón Cancelar
            btnCancelar = new Button
            {
                Text = "❌ CANCELAR",
                Size = new Size(120, 40),
                Location = new Point(190, 20),
                BackColor = Color.FromArgb(220, 53, 69),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnCancelar.Click += (s, e) => this.Close();

            panelBotones.Controls.AddRange(new Control[] { btnGuardar, btnLimpiar, btnCancelar });
        }

        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos obligatorios
                if (!ValidarCamposObligatorios())
                    return;

                // Validar no más de 4 unidades del mismo modelo
                if (!ValidarLimiteModelo())
                    return;

                // Crear el objeto Auto
                Auto nuevoAuto = new Auto
                {
                    Codigo = txtCodigo.Text.Trim(),
                    Marca = txtMarca.Text.Trim(),
                    Modelo = txtModelo.Text.Trim(),
                    Ano = (int)numAno.Value,
                    Color = txtColor.Text.Trim(),
                    Tipo = cmbTipo.SelectedItem?.ToString(),
                    Precio = numPrecio.Value,
                    Cilindraje = numCilindraje.Value,
                    Transmision = cmbTransmision.SelectedItem?.ToString(),
                    Combustible = cmbCombustible.SelectedItem?.ToString(),
                    Puertas = (int)numPuertas.Value,
                    Pasajeros = (int)numPasajeros.Value,
                    Descripcion = txtDescripcion.Text.Trim(),
                    Disponible = true
                };

                // Guardar en memoria
                _controller.AgregarAuto(nuevoAuto);

                // Guardar en la base de datos
                bd.InsertarAuto(nuevoAuto);

                ;

                // Mostrar mensaje de éxito
                MessageBox.Show($"✅ Vehículo agregado exitosamente!\n\n" +
                              $"Código: {nuevoAuto.Codigo}\n" +
                              $"Marca: {nuevoAuto.Marca}\n" +
                              $"Modelo: {nuevoAuto.Modelo}\n" +
                              $"Precio: ${nuevoAuto.Precio:N2}",
                              "Vehículo Agregado",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);

                // Cerrar el formulario
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al guardar el vehículo:\n{ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
            }
        }


        private void BtnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private bool ValidarCamposObligatorios()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MostrarError("El código es obligatorio", txtCodigo);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMarca.Text))
            {
                MostrarError("La marca es obligatoria", txtMarca);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtModelo.Text))
            {
                MostrarError("El modelo es obligatorio", txtModelo);
                return false;
            }

            if (numPrecio.Value <= 0)
            {
                MostrarError("El precio debe ser mayor a 0", numPrecio);
                return false;
            }

            return true;
        }

        private bool ValidarLimiteModelo()
        {
            string modelo = txtModelo.Text.Trim();

            // Contar cuántos autos del mismo modelo existen
            var autosMismoModelo = _controller.Autos.Count(a => a.Modelo == modelo);

            if (autosMismoModelo >= 4)
            {
                MessageBox.Show($"❌ No se pueden agregar más de 4 unidades del modelo '{modelo}'.\n\n" +
                              $"Actualmente hay {autosMismoModelo} unidades en el inventario.",
                              "Límite de Modelo Alcanzado",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void MostrarError(string mensaje, Control control)
        {
            MessageBox.Show(mensaje, "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
        }

        private void LimpiarFormulario()
        {
            txtCodigo.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtColor.Text = "";
            txtDescripcion.Text = "";

            numAno.Value = DateTime.Now.Year;
            numPrecio.Value = 25000;
            numCilindraje.Value = 1800;
            numPuertas.Value = 4;
            numPasajeros.Value = 5;

            if (cmbTipo.Items.Count > 0) cmbTipo.SelectedIndex = 0;
            if (cmbTransmision.Items.Count > 0) cmbTransmision.SelectedIndex = 0;
            if (cmbCombustible.Items.Count > 0) cmbCombustible.SelectedIndex = 0;

            txtCodigo.Focus();
        }
    }
}