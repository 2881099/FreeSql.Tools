namespace FreeSql.EntityGenerator {
	partial class FrmView {
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent() {
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.btnOk = new System.Windows.Forms.Button();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.dgvColIcon = new System.Windows.Forms.DataGridViewImageColumn();
			this.dgvColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvColCsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvColDBType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.dgvColAllowDBNull = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.dgvColView = new System.Windows.Forms.DataGridViewLinkColumn();
			this.dgvColViewComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(437, 589);
			this.btnOk.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(135, 38);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "Ok";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F);
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColIcon,
            this.dgvColName,
            this.dgvColCsName,
            this.dgvColDBType,
            this.dgvColAllowDBNull,
            this.dgvColView,
            this.dgvColViewComment});
			dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F);
			dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
			this.dataGridView1.Location = new System.Drawing.Point(20, 19);
			this.dataGridView1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle5;
			this.dataGridView1.RowTemplate.Height = 23;
			this.dataGridView1.Size = new System.Drawing.Size(930, 557);
			this.dataGridView1.TabIndex = 0;
			// 
			// dgvColIcon
			// 
			this.dgvColIcon.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle2.NullValue = null;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
			this.dgvColIcon.DefaultCellStyle = dataGridViewCellStyle2;
			this.dgvColIcon.HeaderText = "  ";
			this.dgvColIcon.Name = "dgvColIcon";
			this.dgvColIcon.ReadOnly = true;
			this.dgvColIcon.Width = 21;
			// 
			// dgvColName
			// 
			this.dgvColName.HeaderText = "DbName";
			this.dgvColName.Name = "dgvColName";
			this.dgvColName.ReadOnly = true;
			this.dgvColName.Width = 180;
			// 
			// dgvColCsName
			// 
			this.dgvColCsName.HeaderText = "CsName";
			this.dgvColCsName.Name = "dgvColCsName";
			this.dgvColCsName.ReadOnly = true;
			this.dgvColCsName.Width = 180;
			// 
			// dgvColDBType
			// 
			this.dgvColDBType.HeaderText = "SqlType";
			this.dgvColDBType.Name = "dgvColDBType";
			this.dgvColDBType.ReadOnly = true;
			this.dgvColDBType.Width = 130;
			// 
			// dgvColAllowDBNull
			// 
			this.dgvColAllowDBNull.HeaderText = "DBNull";
			this.dgvColAllowDBNull.Name = "dgvColAllowDBNull";
			this.dgvColAllowDBNull.ReadOnly = true;
			this.dgvColAllowDBNull.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvColAllowDBNull.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvColAllowDBNull.Width = 60;
			// 
			// dgvColView
			// 
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
			this.dgvColView.DefaultCellStyle = dataGridViewCellStyle3;
			this.dgvColView.HeaderText = "Relation";
			this.dgvColView.Name = "dgvColView";
			this.dgvColView.ReadOnly = true;
			this.dgvColView.Resizable = System.Windows.Forms.DataGridViewTriState.True;
			this.dgvColView.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
			this.dgvColView.Text = "View";
			this.dgvColView.Width = 60;
			// 
			// dgvColViewComment
			// 
			this.dgvColViewComment.HeaderText = "Comment";
			this.dgvColViewComment.Name = "dgvColViewComment";
			this.dgvColViewComment.ReadOnly = true;
			this.dgvColViewComment.Width = 230;
			// 
			// FrmView
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 19F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOk;
			this.ClientSize = new System.Drawing.Size(970, 641);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.btnOk);
			this.Font = new System.Drawing.Font("宋体", 14F);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
			this.MaximizeBox = false;
			this.Name = "FrmView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FreeSql";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		public System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.DataGridViewImageColumn dgvColIcon;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvColName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvColCsName;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvColDBType;
		private System.Windows.Forms.DataGridViewCheckBoxColumn dgvColAllowDBNull;
		private System.Windows.Forms.DataGridViewLinkColumn dgvColView;
		private System.Windows.Forms.DataGridViewTextBoxColumn dgvColViewComment;
	}
}