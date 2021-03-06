USE HospitalF
GO
-- CHANGE SIZE OF Degree and Experience
ALTER TABLE Doctor
ALTER COLUMN Degree NVARCHAR(1000)
ALTER TABLE Doctor
ALTER COLUMN Experience NVARCHAR(1000)

INSERT INTO [HospitalF].[dbo].[Doctor]
           ([First_Name]
           ,[Last_Name]
           ,[Gender]
           ,[Degree]
           ,[Experience]
           ,[Working_Day]
           ,[Photo_ID]
           ,[Is_Active])
     VALUES
		--Nguyen Huu Nghia doctor
		(N'Nghĩa',N'Nguyễn Hữu',0,N'Bác sĩ Đa khoa - hệ chính quy Đại học Y Dược Tp.HCM năm 2011
Hoàn thành chứng chỉ:
+Phẫu thuật nội soi mũi xoang.
+Phẫu thuật tạo hình thẩm mỹ.
Bs. Nguyễn Xuân Sơn là thành viên Hội viên Hội Tai - Mũi - Họng Tp.HCM',N'Kinh nghiệm hơn 12 năm điều trị, phẫu thuật trong lĩnh vực Tai – Mũi – Họng.','2,3,4,6,7',NULL,1),
		--Bùi Nguyễn Kim Long doctor
		(N'Long',N'Bùi Nguyễn Kim',1,N'Bác sĩ Đa khoa - Đại học Y Phạm Ngọc Thạch năm 1999
Chuyên khoa I Sản phụ khoa - Đại học Y Dược năm 2004.
Hoàn thành chứng chỉ: siêu am thực hành, phẫu thuật nội soi ổ bụng, soi cổ tử cung…',N'Hơn 9 năm kinh nghiệm:
- Điều trị các loại bệnh lý: viêm nhiễm, u bướu, vú, phụ khoa.
- Thực hiện phẫu thuật: Rạch abus vú, sinh thiết u vú; Bóc kyste vùng âm hộ; Phẫu thuật thai ngoài tử cung, mổ lấy thai; U nang buồng trứng, u xơ tử cung…
- Tư vấn, điều trị các rối loạn tiền mãn kinh, mãn kinh.
- Tư vấn thực hiện kế hoạch hóa gia đình.','2,3,6,7',NULL,1),
		--Võ Ngọc Thoại Trâm doctor
		(N'Trâm',N'Võ Ngọc Thoại',0,N'Bác sĩ Y khoa, Đại học Paris V, Pháp (1989)
Chứng chỉ đào tạo Bệnh nhiệt đói, Đại học Paris V, Pháp (1989)
Bằng chuyên khoa sau đại học, khoa Ung thu vùng Mặt và Cổ, Học viện Gustave Roussy, Pháp (1993)
Bằng chuyên khoa sau đại học, khoa Phẫu thuật Miệng và Hàm mặt, Đại học Routen, Pháp (1995)
Bằng chuyên khoa sau đại học, khoa Chỉnh nha, Đại học Paul Sabatier, Toulouse, Pháp (1996)
Bằng chuyên khoa sau đại học, khoa Vi phẫu, Đại học Rouen, Pháp (1996)
Bằng chuyên khoa sâu, khoa Chỉnh nha, đại học Routen, Routen, Pháp (1998)
Bằng chuyên khoa sau đại học, khoa Cấy ghép nha, Đại học Paul Sabatier, Toulouse, Pháp (2011)',N'Bác sĩ phẫu thuật, khoa Phẫu thuật Miệng và Hàm mặt, Bệnh viện Evreux, Everux, Pháp (1998-2000)
Bác sĩ phẫu thuật, khoa Miệng và Hàm mặt, bệnh viện Pitié-Salpêtrière, Pháp (2001-2009)
Bác sĩ trưởng khoa khoa Răng-hàm-mặt, bệnh viện Hoàn Mỹ (2009 đến nay)','2,3,4,6',NULL,1),
		--Nguyen Ngọc Hiến doctor
		(N'Hiến',N'Nguyễn Ngọc',0,N'Bằng Trung học Kỹ thuật Y Tế TW3 - Tp.Hồ Chí Minh - 1995',N'Kỹ thuật viên Vật lý trị liệu Trung tâm Chấn thương Chỉnh Hình, Tp. Hồ Chí Minh, 1995-1997.
Kỹ thuật viên Vật lý trị liệu tại Cơ sở tư nhân Vật lý Trị Liệu - Tp. Hồ Chí Minh, 1998-2007.
Kỹ thuật viên Vật lý trị liệu - Khoa Vật ký trị liệu - Bệnh Viện Hoàn Mỹ Sài Gòn, từ 2007 đến nay.
Kỹ thuật viên Vật lý trị liệu - Khoa Vật ký trị liệu - Bệnh Viện FV, Tp. Hồ Chí Minh, từ 2009 đến nay.','2,3,5,6',NULL,1),
		--Pham Hoang Long doctor
		(N'Long',N'Phạm Hoàng',0,N'Bác sĩ y khoa - Doctor of Medicine (M.D.), Đại học Y khoa Georgetown, Washington DC, USA.
Chứng chỉ, U.S. National Board of Medical Examiners
Chứng chỉ, Hiệp hội Nhãn khoa Mỹ (American Board of Ophthalmology)
Chứng chỉ, Hội đồng y khoa Virginia (Virginia Board of Medicine)
Chứng chỉ, Hội đồng y khoa California (California Board of Medicine)',N'Giám đốc Y khoa Phòng khám chuyên khoa Mắt American Eye Center, có hơn 17 năm kinh nghiệm.
Thành viên Viện Nhãn khoa Mỹ (American Academy of Ophthalmology)
Thành viên Hiệp hội Phẫu thuật các bệnh khúc xạ và đục thủy tinh thể Mỹ (American Society of Cataract and Refractive Surgeons)
Bác sĩ Trung Tá Quân y, Không Lực Hoa Kỳ, Bệnh viện Walter Reed và Malcolm Grow Medical Center','2,3,4,5,6,7,8',NULL,1),
		-- Le Thi Kim Ngan doctor
		(N'Ngân',N'Lê Thị Kim',1,N'Tốt nghiệp Đại học Y Dược Tp.HCM:
	- Bs. Đa khoa năm 1996.
	Chuyên khoa I Sản phụ khoa năm 1999. 
	- Hoàn thành chứng chỉ:
	+ Phẫu thuật nội soi cơ bản năm 2003.
	+ Phẫu thuật nội soi nâng cao năm 2010.
	+ Phẫu thuật phục hồi sàn chậu năm 2011.',N'Hiện là Trưởng khoa – Khoa Sản phụ khoa, Bs. Kim Ngân có hơn 14 năm kinh nghiệm điều trị các bệnh lý phụ khoa.
	Chuyên điều trị các bệnh lý: Viêm nhiễm trùng phụ khoa, bệnh u nang lành tính của vú; u xơ tử cung, u nang buồng trứng, thai ngoài tử cung, nang tuyến Bartholin; Rối loạn kinh nguyệt: rong kinh, rong huyết, cường kinh, thống kinh...
	- Chuyên phẫu thuật: Nội soi bóc u xơ tử cung, u nang buồng trứng, cắt tử cung; Phục hồi sàn chậu; Nội soi thai ngoài tử cung; Bóc tuyến bartholine, bóc u vú, may thẩm mỹ tầng sinh môn… 
	Kế hoạch hóa gia đình: Đặt lấy vòng tránh thai, cấy/lấy que ngừa thai.','2,3,4,5,6,7',NULL,1),
	-- Hoang Ngoc Duc doctor
		(N'Đức',N'Hoàng Ngọc',0,N'- Thạc sĩ Y Học, ngành Tai Mũi Họng – Đại học Y Dược Tp.HCM năm 1999. 
	- Hoàn thành chứng chỉ:
	+ Phẫu thuật nội soi mũi xoang cơ bản.
	+ Phẫu thuật vi phẫu thanh quản, cắt amidan bằng máy coblator, + Phẫu thuật, điều trị ngủ ngáy.
	+ Nâng cao phẫu thuật tai, phẫu tích xương thái dương.
	Ths. Bs. Hoàng Ngọc Đức là thành viên của Hội Viên Hội Tai Mũi Họng Việt Nam, Hội Viên Hội Tai Mũi Họng Nhi Việt Nam.',N'Hơn 18 năm kinh nghiệm điều trị, phẫu thuật: 
	Tai: viêm tai ngoài và giữa, viêm tai xương chủm,… 
	Mũi: viêm mũi xoang, vẹo vách ngăn, u hốc mũi, chỉnh hình vách ngăn,...
	Họng: viêm họng, viêm amiđan mạn, viêm va, viêm thanh quản, hạt dây thanh, polyp dây thanh, cắt amiđan mạn, nạo va, soi treo thanh quản cắt hạt dây thanh, chỉnh hình màn hầu, ung thư thanh quản,... Đầu mặt cổ: u vùng cổ, nang giáp lưỡi, ... 
	Điều trị ngáy.','2,3,4,5',NULL,1),
	-- Phan Du Le Loi doctor
		(N'Lợi',N'Phan Dư Lê',0,N'- Bác sĩ Đa khoa – Hệ chính quy – Đại học Y Dược Tp.HCM năm 2011.
	- Hoàn thành chứng chỉ:
	+ Phẫu thuật nội soi mũi xoang.
	+ Phẫu thuật tạo hình thẩm mỹ.
	Bs. Phan Dư Lê Lợi là thành viên của Hội viện Hội Tai – Mũi – Họng Tp.HCM',N'Kinh nghiệm hơn 12 năm điều trị, phẫu thuật trong lĩnh vực Tai – Mũi – Họng.','2,3,4,6',NULL,1),
	-- Do Tuyet Loan doctor
		(N'Loan',N'Đỗ Tuyết',1,N'2001 : Tốt nghiệp Răng-Hàm-Mặt tại Đại học Y Dược,TPHCM',N'Có 11 năm kinh nghiệm về Răng - Hàm - Mặt 
	- 2003 : Công tác tại các phòng khám nha khoa Quận 5,10 
	- 2012 đến nay: công tác tại Bệnh viện Hoàn Mỹ','2,4,6',NULL,1),
	-- Nguyen Thi Minh Hanh doctor
		(N'Hạnh',N'Nguyễn Thị Minh',1,N'2004 : Tốt nghiệp Răng-Hàm-Mặt tại Đại học Y Dược,TP.HCM',N'Có 9 năm kinh nghiệm trong lĩnh vực Răng-Hàm-Mặt 
	2005 đến nay: Công tác tại Bệnh viện Hoàn Mỹ','2,3,5,6,8',NULL,1),
	(N'Hồng',N'Nguyễn Thị Thu',1,N'2010 : Chứng chỉ Phẫu thuật Nội soi cơ bản trong Phụ khoa tại Bệnh viện Từ Dũ,TP.HCM 
	2011 : Tốt nghiệp chuyên khoa 1 Sơ bộ chuyên khoa Sản phụ khoa tại Bệnh viện Đại học Y Dược,TP.HCM 
	2014 : Chứng chỉ siêu âm sản phụ khoa cơ bản tại Bệnh viện Từ Dũ, TP.HCM',N'Có hơn 10 năm kinh nghiệm trong khám và điều trị các bệnh lý sản phụ khoa.
	 2002: Công tác tại Bệnh viện Đa khoa Chơn Thành - Bình Phước 
	2009 đến nay : Công tác tại Bệnh viện Hoàn Mỹ','2,3,4,5,6,7,8',NULL,1),
	-- Nguyen Thi Thanh Thuy doctor
		(N'Thủy',N'Nguyễn Thị Thanh',1,N'1993 : Tốt nghiệp Y khoa tại Đại học Y Huế 
	2003 : Thạc sĩ Y học tại Đại học Y Dược,TP.HCM 
	2012 : Chứng chỉ siêu âm Sản phụ khoa tại Đại học Y khoa Phạm Ngọc Thạch',N'Có hơn 30 năm kinh nghiệm trong khám và điều trị các bệnh lý sản phụ khoa. 
	1993 : Làm việc tại Bệnh viện Tư Nghĩa,Quãng Ngãi 
	2002 đến nay : Làm việc tại Bệnh viện Hoàn Mỹ','2,3,4,5,6,7,8',NULL,1)


GO