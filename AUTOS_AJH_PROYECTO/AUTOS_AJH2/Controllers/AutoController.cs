using AUTOS_AJH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace AUTOS_AJH.Controllers
{
    public class AutoController
    {
        private List<Auto> _autos;
        private List<Venta> _ventas;
        private readonly string rutaAutos = Path.Combine(Application.StartupPath, "autos.json");
        private readonly string rutaVentas = Path.Combine(Application.StartupPath, "ventas.json");

        public List<Auto> Autos => _autos;
        public List<Venta> Ventas => _ventas;

        public AutoController()
        {
            _autos = new List<Auto>();
            _ventas = new List<Venta>();
            CargarDatos();
        }

        private void CargarDatos()
        {
            try
            {
                // Cargar autos desde archivo
                if (File.Exists(rutaAutos))
                {
                    string jsonAutos = File.ReadAllText(rutaAutos);
                    _autos = JsonConvert.DeserializeObject<List<Auto>>(jsonAutos) ?? new List<Auto>();
                }
                else
                {
                    // Si no existe el archivo, cargar datos de prueba y guardar
                    CargarDatosPrueba();
                    GuardarAutos();
                }

                // Cargar ventas desde archivo
                if (File.Exists(rutaVentas))
                {
                    string jsonVentas = File.ReadAllText(rutaVentas);
                    _ventas = JsonConvert.DeserializeObject<List<Venta>>(jsonVentas) ?? new List<Venta>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}\nSe cargarán datos de prueba.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CargarDatosPrueba();
            }
        }

        private void GuardarAutos()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_autos, Formatting.Indented);
                File.WriteAllText(rutaAutos, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar autos: {ex.Message}");
            }
        }

        private void GuardarVentas()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_ventas, Formatting.Indented);
                File.WriteAllText(rutaVentas, json);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al guardar ventas: {ex.Message}");
            }
        }

        private void CargarDatosPrueba()
        {
            _autos.Add(new Auto
            {
                Codigo = "TOY001",
                Marca = "Toyota",
                Modelo = "Corolla",
                Ano = 2023,
                Color = "Blanco Perla",
                Precio = 28500,
                Cilindraje = 1800,
                Tipo = "Sedán",
                Transmision = "Automática",
                Combustible = "Gasolina",
                Puertas = 4,
                Pasajeros = 5,
                Descripcion = "Sedán confiable y eficiente en combustible.",
                Disponible = true
            });

            _autos.Add(new Auto
            {
                Codigo = "HON002",
                Marca = "Honda",
                Modelo = "CR-V",
                Ano = 2024,
                Color = "Gris Plata",
                Precio = 36500,
                Cilindraje = 2400,
                Tipo = "SUV",
                Transmision = "Automática",
                Combustible = "Híbrido",
                Puertas = 4,
                Pasajeros = 5,
                Descripcion = "SUV familiar con tecnología híbrida.",
                Disponible = true
            });

            _autos.Add(new Auto
            {
                Codigo = "FOR003",
                Marca = "Ford",
                Modelo = "Mustang",
                Ano = 2022,
                Color = "Rojo Racing",
                Precio = 45200,
                Cilindraje = 5000,
                Tipo = "Deportivo",
                Transmision = "Manual",
                Combustible = "Gasolina",
                Puertas = 2,
                Pasajeros = 4,
                Descripcion = "Deportivo americano icónico.",
                Disponible = true
            });
        }

        public bool AgregarAuto(Auto auto)
        {
            try
            {
                if (_autos.Any(a => a.Codigo == auto.Codigo))
                    throw new Exception("❌ El código del auto ya existe");

                var countModelo = _autos.Count(a => a.Modelo == auto.Modelo);
                if (countModelo >= 4)
                    throw new Exception($"❌ No se pueden agregar más de 4 unidades del modelo {auto.Modelo}");

                if (!auto.EsValido())
                    throw new Exception("❌ Datos del auto incompletos o inválidos");

                _autos.Add(auto);
                GuardarAutos(); // Guardar después de agregar
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al agregar auto: {ex.Message}");
            }
        }

        public bool EliminarAuto(string codigo)
        {
            try
            {
                var auto = _autos.FirstOrDefault(a => a.Codigo == codigo);
                if (auto != null)
                {
                    _autos.Remove(auto);
                    GuardarAutos(); // Guardar después de eliminar
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al eliminar auto: {ex.Message}");
            }
        }

        public bool ActualizarAuto(Auto autoActualizado)
        {
            try
            {
                var autoExistente = _autos.FirstOrDefault(a => a.Codigo == autoActualizado.Codigo);
                if (autoExistente != null)
                {
                    // Actualizar todas las propiedades
                    autoExistente.Marca = autoActualizado.Marca;
                    autoExistente.Modelo = autoActualizado.Modelo;
                    autoExistente.Ano = autoActualizado.Ano;
                    autoExistente.Color = autoActualizado.Color;
                    autoExistente.Precio = autoActualizado.Precio;
                    autoExistente.Cilindraje = autoActualizado.Cilindraje;
                    autoExistente.Tipo = autoActualizado.Tipo;
                    autoExistente.Transmision = autoActualizado.Transmision;
                    autoExistente.Combustible = autoActualizado.Combustible;
                    autoExistente.Puertas = autoActualizado.Puertas;
                    autoExistente.Pasajeros = autoActualizado.Pasajeros;
                    autoExistente.Descripcion = autoActualizado.Descripcion;
                    autoExistente.Disponible = autoActualizado.Disponible;

                    GuardarAutos(); // Guardar después de actualizar
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar auto: {ex.Message}");
            }
        }

        public List<Auto> ObtenerAutosDisponibles()
        {
            return _autos.Where(a => a.Disponible).ToList();
        }

        public Auto BuscarAutoPorCodigo(string codigo)
        {
            return _autos.FirstOrDefault(a => a.Codigo == codigo && a.Disponible);
        }

        public bool RegistrarVenta(string codigoAuto, decimal descuento, string vendedor, string cliente)
        {
            try
            {
                var auto = BuscarAutoPorCodigo(codigoAuto);
                if (auto == null)
                    throw new Exception("Auto no encontrado o no disponible");

                if (descuento < 0 || descuento > auto.Precio)
                    throw new Exception("Descuento inválido");

                auto.Disponible = false;

                var venta = new Venta
                {
                    CodigoAuto = codigoAuto,
                    PrecioVenta = auto.Precio,
                    DescuentoAplicado = descuento,
                    PrecioFinal = auto.Precio - descuento,
                    Vendedor = vendedor,
                    Cliente = cliente,
                    FechaVenta = DateTime.Now
                };

                _ventas.Add(venta);

                // Guardar ambos cambios
                GuardarAutos();
                GuardarVentas();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al registrar venta: {ex.Message}");
            }
        }

        public bool AplicarDescuento(string codigoAuto, decimal porcentajeDescuento)
        {
            try
            {
                if (porcentajeDescuento > 10)
                    throw new Exception("El descuento no puede ser mayor al 10%");

                var auto = BuscarAutoPorCodigo(codigoAuto);
                if (auto == null) return false;

                decimal descuento = auto.Precio * (porcentajeDescuento / 100);
                auto.Precio -= descuento;

                GuardarAutos(); // Guardar después de aplicar descuento
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al aplicar descuento: {ex.Message}");
            }
        }

        // Consultas especiales
        public Auto ObtenerAutoMasAntiguo()
        {
            return _autos.Where(a => a.Disponible).OrderBy(a => a.Ano).FirstOrDefault();
        }

        public Auto ObtenerAutoMayorCilindraje()
        {
            return _autos.Where(a => a.Disponible).OrderByDescending(a => a.Cilindraje).FirstOrDefault();
        }

        public Auto ObtenerAutoPrecioMasBajo()
        {
            return _autos.Where(a => a.Disponible).OrderBy(a => a.Precio).FirstOrDefault();
        }

        public List<string> ObtenerMarcas()
        {
            return _autos.Select(a => a.Marca).Distinct().ToList();
        }

        public List<string> ObtenerTipos()
        {
            return _autos.Select(a => a.Tipo).Distinct().ToList();
        }

        public int TotalAutosDisponibles()
        {
            return _autos.Count(a => a.Disponible);
        }

        public decimal ValorTotalInventario()
        {
            return _autos.Where(a => a.Disponible).Sum(a => a.Precio);
        }

        // Método para forzar guardado manual si es necesario
        public void GuardarTodo()
        {
            GuardarAutos();
            GuardarVentas();
        }

        // Método para hacer backup de los datos
        public void HacerBackup(string rutaBackup)
        {
            try
            {
                string backupAutos = Path.Combine(rutaBackup, $"autos_backup_{DateTime.Now:yyyyMMdd_HHmmss}.json");
                string backupVentas = Path.Combine(rutaBackup, $"ventas_backup_{DateTime.Now:yyyyMMdd_HHmmss}.json");

                if (File.Exists(rutaAutos))
                    File.Copy(rutaAutos, backupAutos, true);

                if (File.Exists(rutaVentas))
                    File.Copy(rutaVentas, backupVentas, true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear backup: {ex.Message}");
            }
        }
    }
}