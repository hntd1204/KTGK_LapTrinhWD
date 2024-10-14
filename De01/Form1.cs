using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using De01.Model;

namespace De01
{
    public partial class Form1 : Form
    {
        DbStudentContent dbStudent = new DbStudentContent();
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "" || txtHoTenSV.Text == "" || cboLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo");
                return;
            }

            var sinhvienCanSua = dbStudent.Sinhviens.Find(txtMaSV.Text);
            if (sinhvienCanSua == null)
            {
                MessageBox.Show("Không tìm thấy sinh viên cần sửa.", "Thông báo");
                return;
            }

            sinhvienCanSua.HotenSV = txtHoTenSV.Text;
            sinhvienCanSua.NgaySinh = dtNgaySinh.Value;
            sinhvienCanSua.MaLop = cboLop.SelectedValue.ToString();

            dbStudent.SaveChanges();

            FillDataDGV(dbStudent.Sinhviens.ToList());

            txtMaSV.Clear();
            txtHoTenSV.Clear();
            cboLop.SelectedIndex = -1;
            dtNgaySinh.Value = DateTime.Now;

            MessageBox.Show("Cập nhật thông tin sinh viên thành công.", "Thông báo");
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<Sinhvien> listSinhVien = dbStudent.Sinhviens.ToList();
            List<Lop> listLop = dbStudent.Lops.ToList();

            FillDataCBB(listLop);
            FillDataDGV(listSinhVien);

            dgvSinhvien.CellClick += dgvSinhvien_CellClick;
        }

        private void FillDataDGV(List<Sinhvien> listSinhVien)
        {
            dgvSinhvien.Rows.Clear();
            foreach (var sinhvien in listSinhVien)
            {
                int RowNew = dgvSinhvien.Rows.Add();
                dgvSinhvien.Rows[RowNew].Cells[0].Value = sinhvien.MaSV;
                dgvSinhvien.Rows[RowNew].Cells[1].Value = sinhvien.HotenSV;
                dgvSinhvien.Rows[RowNew].Cells[2].Value = sinhvien.NgaySinh;
                dgvSinhvien.Rows[RowNew].Cells[3].Value = sinhvien.Lop.TenLop;

            }
        }

        private void FillDataCBB(List<Lop> listLop)
        {
            cboLop.DataSource = listLop;

            cboLop.DisplayMember = "TenLop";

            cboLop.ValueMember = "MaLop";
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "" || txtHoTenSV.Text == "" || cboLop.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo");
                return;
            }

            Sinhvien sinhvienMoi = new Sinhvien
            {
                MaSV = txtMaSV.Text,
                HotenSV = txtHoTenSV.Text,
                NgaySinh = dtNgaySinh.Value,
                MaLop = cboLop.SelectedValue.ToString()
            };

            var sinhvienTonTai = dbStudent.Sinhviens.Find(sinhvienMoi.MaSV);
            if (sinhvienTonTai != null)
            {
                MessageBox.Show("Mã sinh viên đã tồn tại. Vui lòng nhập mã khác.", "Thông báo");
                return;
            }

            try
            {
                dbStudent.Sinhviens.Add(sinhvienMoi);
                dbStudent.SaveChanges();

                FillDataDGV(dbStudent.Sinhviens.ToList());

                txtMaSV.Clear();
                txtHoTenSV.Clear();
                cboLop.SelectedIndex = -1;
                dtNgaySinh.Value = DateTime.Now;

                MessageBox.Show("Thêm sinh viên thành công.", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra khi thêm sinh viên: {ex.Message}", "Thông báo");
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            if (txtMaSV.Text == "")
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên cần xóa.", "Thông báo");
                return;
            }

            var sinhvienCanXoa = dbStudent.Sinhviens.Find(txtMaSV.Text);
            if (sinhvienCanXoa == null)
            {
                MessageBox.Show("Không tìm thấy sinh viên cần xóa.", "Thông báo");
                return;
            }

            var result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                return;
            }

            dbStudent.Sinhviens.Remove(sinhvienCanXoa);
            dbStudent.SaveChanges();

            FillDataDGV(dbStudent.Sinhviens.ToList());

            txtMaSV.Clear();
            txtHoTenSV.Clear();
            cboLop.SelectedIndex = -1;
            dtNgaySinh.Value = DateTime.Now;

            MessageBox.Show("Xóa sinh viên thành công.", "Thông báo");
        }

        private void dgvSinhvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvSinhvien.Rows[e.RowIndex];

            txtMaSV.Text = row.Cells[0].Value.ToString();
            txtHoTenSV.Text = row.Cells[1].Value.ToString();
            dtNgaySinh.Value = Convert.ToDateTime(row.Cells[2].Value);
            cboLop.Text = row.Cells[3].Value.ToString();
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
