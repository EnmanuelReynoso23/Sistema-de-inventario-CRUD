using System.Data;
using System.Drawing.Printing;
using System.Globalization;

namespace Proyecto
{
    // Formulario principal de la aplicación de gestión de inventario.
    // Contiene la lógica de UI para crear, actualizar, eliminar y manipular
    // registros de productos en un DataGridView (DGV).
    public partial class mainForm : Form
    {
        // Campos para gestionar pantalla completa
        private bool isFullScreen = false;
        private FormBorderStyle prevBorderStyle;
        private Rectangle prevBounds;

        public mainForm()
        {
            InitializeComponent();

            //capturar teclas a nivel de formulario
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
        }


        // Para alternar entre pantalla completa y ventana pequeña

        private void ToggleFullScreen()
        {
            if (!isFullScreen)
            {
                prevBorderStyle = this.FormBorderStyle;
                prevBounds = this.Bounds;

                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Normal; // asegurar reasignación de Bounds
                this.Bounds = Screen.FromControl(this).Bounds;
                this.WindowState = FormWindowState.Maximized;

                isFullScreen = true;
                SSLEstado!.Text = "Pantalla completa (F11 para alternar, Esc para salir)";
            }
            else
            {
                this.FormBorderStyle = prevBorderStyle;
                this.WindowState = FormWindowState.Normal;
                this.Bounds = prevBounds;

                isFullScreen = false;
                SSLEstado!.Text = "Modo ventana";
            }
        }

        // Maneja F11 para alternar y Esc para salir de pantalla completa
        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                ToggleFullScreen();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape && isFullScreen)
            {
                ToggleFullScreen();
                e.Handled = true;
            }
        }
        // Evento: se ejecuta cuando cambia el texto del campo ID.
        // Valida que el ID sea numérico y muestra un mensaje en la barra de estado (SSLEstado).
        private void TxtID_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtID!.Text) && !int.TryParse(txtID!.Text, out _))
                SSLEstado!.Text = "ID inválido, debe ser numérico";
            else
                SSLEstado!.Text = "";
        }

        // Evento: se ejecuta cuando cambia el texto del campo Nombre.
        // Indica si el nombre está vacío (campo obligatorio).
        private void TxtNombre_TextChanged(object? sender, EventArgs e)
        {
            SSLEstado.Text = string.IsNullOrWhiteSpace(TxtNombre!.Text) ? "El nombre es obligatorio" : "";
        }

        // Evento: se ejecuta cuando cambia el texto del campo Precio.
        // Valida que el precio sea un decimal > 0 y actualiza la barra de estado.
        private void TxtPrecio_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtPrecio!.Text))
            {
                if (!decimal.TryParse(TxtPrecio!.Text, out var p) || p <= 0)
                    SSLEstado!.Text = "Precio inválido";
                else
                    SSLEstado!.Text = "";
            }
            else SSLEstado!.Text = "";
        }

        // Evento: se ejecuta cuando cambia el texto del campo Stock.
        // Valida que sea un entero mayor o igual a 0.
        private void TxtStock_TextChanged(object? sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtStock!.Text))
            {
                if (!int.TryParse(TxtStock!.Text, out var s) || s < 0)
                    SSLEstado!.Text = "Stock inválido";
                else
                    SSLEstado!.Text = "";
            }
            else SSLEstado!.Text = "";
        }

        // Botón Crear: crea una nueva fila en el DataGridView después de validar campos.
        // Se muestran mensajes emergentes (MessageBox) en caso de error de validación.
        private void BtnCrear_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNombre!.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }
            if (!decimal.TryParse(TxtPrecio!.Text, out var precio) || precio <= 0)
            {
                MessageBox.Show("Precio inválido. Debe ser un número mayor que 0.");
                return;
            }
            if (!int.TryParse(TxtStock!.Text, out var stock) || stock < 0)
            {
                MessageBox.Show("Stock inválido. Debe ser un entero, mayor o igual a 0.");
                return;
            }

            // Genera un ID unico para cada producto (no duplicable, acepta multiples filas con mismo ID y Nombre)
            string nombreKey = (TxtNombre!.Text ?? "").Trim();
            int idToUse = 0;

            foreach (DataGridViewRow r in DGV!.Rows)
            {
                if (r.IsNewRow) continue;

                string nom = (Convert.ToString(r.Cells[1].Value) ?? "").Trim();
                if (string.Equals(nom, nombreKey, StringComparison.OrdinalIgnoreCase))
                {
                    string sId = Convert.ToString(r.Cells[0].Value) ?? "";
                    string digits = new string(sId.Where(char.IsDigit).ToArray());
                    int.TryParse(digits, out idToUse);
                    break;
                }
            }

            if (idToUse <= 0)
            {
                int maxId = 0;
                foreach (DataGridViewRow r in DGV!.Rows)
                {
                    if (r.IsNewRow) continue;
                    string sId = Convert.ToString(r.Cells[0].Value) ?? "";
                    string digits = new string(sId.Where(char.IsDigit).ToArray());
                    if (int.TryParse(digits, out var v) && v > maxId) maxId = v;
                }
                idToUse = Math.Max(maxId + 1, 100000);
            }

            txtID!.Text = idToUse.ToString();

            // Agrega la nueva fila; se formatea el precio y el stock antes de añadir.
            DGV!.Rows.Add(txtID!.Text, TxtNombre!.Text.Trim(), precio.ToString("0.##"), stock.ToString(), DTP!.Value.ToString("dd/MM/yyyy"));
            SSLEstado!.Text = "Producto creado";
        }

        // Botón Actualizar: actualiza la fila actualmente seleccionada en el DGV.
        // Valida los mismos campos que crear y sobrescribe los valores de las celdas.
        private void BtnActualizar_Click(object? sender, EventArgs e)
        {
            if (DGV!.CurrentRow == null || DGV!.CurrentRow.IsNewRow)
            {
                MessageBox.Show("Seleccione una fila para actualizar.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtNombre!.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return;
            }
            if (!decimal.TryParse(TxtPrecio!.Text, out var precio) || precio <= 0)
            {
                MessageBox.Show("Precio inválido. Debe ser un número mayor que 0.");
                return;
            }
            if (!int.TryParse(TxtStock!.Text, out var stock) || stock < 0)
            {
                MessageBox.Show("Stock inválido. Debe ser un entero ≥ 0.");
                return;
            }

            // Actualiza las celdas de la fila seleccionada.
            DGV!.CurrentRow.Cells[0].Value = txtID!.Text;
            DGV!.CurrentRow.Cells[1].Value = TxtNombre!.Text.Trim();
            DGV!.CurrentRow.Cells[2].Value = precio.ToString("0.##");
            DGV!.CurrentRow.Cells[3].Value = stock.ToString();
            DGV!.CurrentRow.Cells[4].Value = DTP!.Value.ToString("dd/MM/yyyy");

            SSLEstado!.Text = "Producto actualizado";
        }

        // Evento: cuando se hace click en una celda del DGV, carga los valores en los campos del formulario
        // para poder editar o visualizar el registro.
        private void DGV_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = DGV!.Rows[e.RowIndex];

            txtID!.Text = row.Cells[0].Value?.ToString();
            TxtNombre!.Text = row.Cells[1].Value?.ToString();
            TxtPrecio!.Text = row.Cells[2].Value?.ToString();
            TxtStock!.Text = row.Cells[3].Value?.ToString();
            if (DateTime.TryParseExact(row.Cells[4].Value?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
                DTP!.Value = dt;
            else
                DTP!.Value = DateTime.Now;

            SSLEstado!.Text = "Fila cargada";
        }

        // Botón Eliminar: elimina la fila actualmente seleccionada si existe.
        private void BtnEliminar_Click(object? sender, EventArgs e)
        {

            if (DGV!.CurrentRow == null || DGV!.CurrentRow.IsNewRow) {
                MessageBox.Show("Seleccione un producto para eliminar.", "Advertencia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (confirmaciónEliminación confirmDialog = new confirmaciónEliminación()) {
                if (confirmDialog.ShowDialog() == DialogResult.OK && confirmDialog.Confirmado) {
                    DGV!.Rows.Remove(DGV!.CurrentRow);
                    SSLEstado!.Text = "Producto eliminado";
                } else {
                    SSLEstado!.Text = "Eliminación cancelada";
                }
            }

            /*if (DGV.CurrentRow != null)
            {
                DGV.Rows.Remove(DGV.CurrentRow);
                SSLEstado.Text = "Producto eliminado";
            }*/
        }

        // Botón Limpiar: limpia los campos del formulario y deselecciona filas en el DGV.
        private void BtnLimpiar_Click(object? sender, EventArgs e)
        {
            txtID!.Clear();
            TxtNombre!.Clear();
            TxtPrecio!.Clear();
            TxtStock!.Clear();
            DTP!.Value = DateTime.Now;
            DGV!.ClearSelection();
            SSLEstado!.Text = "Formulario limpio";
        }

        // Menú: Nuevo - reinicia los campos y pone el foco en Nombre.
        private void NuevoToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            txtID!.Clear(); TxtNombre!.Clear(); TxtPrecio!.Clear(); TxtStock!.Clear();
            SSLEstado!.Text = "Nuevo registro";
            TxtNombre!.Focus();

        }

        // Menú: Guarda/Crear mapea al botón Actualizar (se reutiliza la lógica existente).
        private void GuardarCrearToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            btnActualizar.PerformClick();
        }

        private void ActualizarToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            btnActualizar.PerformClick();
        }

        private void EliminarToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            btnEliminar.PerformClick();
        }

        // Menú: Importar CSV - abre un dialogo para seleccionar un archivo CSV y agrega cada línea como fila.
        // NOTA: No hace parsing avanzado ni manejo de comillas/escape; asume columnas separadas por comas.
        private void ImportarCSVToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "CSV|*.csv";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string line in File.ReadAllLines(ofd.FileName))
                    {
                        string[] data = line.Split(',');
                        DGV!.Rows.Add(data);
                    }
                    SSLEstado!.Text = "CSV importado";
                }
            }
        }

        // Menú: Exportar CSV - escribe cada fila del DGV en un archivo CSV seleccionado.
        // Omite la fila nueva que el DataGridView tiene por defecto.
        private void ExportarCSVToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV|*.csv";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(sfd.FileName))
                    {
                        foreach (DataGridViewRow row in DGV!.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                var cells = row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? "");
                                sw.WriteLine(string.Join(",", cells));
                            }
                        }
                    }
                    SSLEstado!.Text = "CSV exportado";
                }
            }
        }

        // Menú: Imprimir listado - muestra un diálogo de impresión. La impresión real no está implementada.
        private void ImprimirListadoToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == DialogResult.OK)
            {
                PrintDocument printDoc = new PrintDocument();
                SSLEstado!.Text = "Enviando a impresora...";
            }
        }

        // Menú: Salir - deshabilita el elemento y cierra la aplicación.
        private void SalirToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            salirToolStripMenuItem!.Enabled = false;
            Application.Exit();
        }

        // Copiar el valor de la celda actual al portapapeles.
        private void CopiarCeldaToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (DGV!.CurrentCell != null)
                Clipboard.SetText(DGV!.CurrentCell.Value?.ToString() ?? "");
        }

        // Copiar la fila actual como una línea CSV al portapapeles.
        private void CopiarFilaComoCSVToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (DGV!.CurrentRow != null)
            {
                var cells = DGV!.CurrentRow.Cells.Cast<DataGridViewCell>()
                             .Select(c => c.Value?.ToString() ?? "");
                Clipboard.SetText(string.Join(",", cells));
            }
        }

        // Limpia la selección del DataGridView.
        private void LimpiarSelecciónToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            DGV!.ClearSelection();
        }

        // Refresca la vista del DataGridView.
        private void RefrescarToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            DGV!.Refresh();
            SSLEstado!.Text = "Vista actualizada";
        }

        // Filtro: muestra solo las filas cuyo stock sea > 0.
        // Campo para controlar el estado del filtro de stock, de desabilita al hacer click nuevamente.
        private bool soloConStockActivo = false;

        private void SoloConStockToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (!soloConStockActivo)
            {
                foreach (DataGridViewRow row in DGV!.Rows)
                {
                    if (row.IsNewRow) continue;
                    var val = row.Cells[3].Value?.ToString();
                    if (!int.TryParse(val, out var stk)) stk = 0;
                    row.Visible = stk > 0;
                }
                SSLEstado!.Text = "Mostrando solo con stock";
                soloConStockActivo = true;
            }
            else
            {
                foreach (DataGridViewRow row in DGV!.Rows)
                {
                    if (row.IsNewRow) continue;
                    row.Visible = true;
                }
                SSLEstado!.Text = "Mostrando todos los productos";
                soloConStockActivo = false;
            }
        }

        // Permite editar el registro seleccionado cargando sus datos en los controles del formulario.
        private void EditarSeleccionadoToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (DGV!.CurrentRow != null)
            {
                txtID!.Text = DGV!.CurrentRow.Cells[0].Value?.ToString();
                TxtNombre!.Text = DGV!.CurrentRow.Cells[1].Value?.ToString();
                TxtPrecio!.Text = DGV!.CurrentRow.Cells[2].Value?.ToString();
                TxtStock!.Text = DGV!.CurrentRow.Cells[3].Value?.ToString();
                DTP!.Value = DateTime.TryParseExact(DGV!.CurrentRow.Cells[4].Value?.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt) ? dt : DateTime.Now;
                SSLEstado!.Text = "Editando registro";
            }
        }

        // Duplicar fila: copia los valores de la fila seleccionada y añade una nueva fila con esos datos.
        private void DuplicarSeleccionadoToolStripMenuItem_Click(object? sender, EventArgs e)
        {
            if (DGV.CurrentRow != null)
            {
                var cells = DGV.CurrentRow.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? "").ToArray();
                DGV.Rows.Add(cells);
                SSLEstado.Text = "Registro duplicado";
            }
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            // Aquí puede inicializar datos al cargar el formulario, si es necesario.
        }

        private void DTP_ValueChanged(object sender, EventArgs e)
        {
            DTP.CustomFormat = "dd/MM/yyyy";
        }

        private void SSLConteo_Click(object sender, EventArgs e)
        {
            int count = 0;
            foreach (DataGridViewRow r in DGV.Rows)
            {
                if (r.IsNewRow) continue;
                if (r.Visible) count++;
            }
            SSLConteo.Text = $"{count} registros";
        }
    }
}
