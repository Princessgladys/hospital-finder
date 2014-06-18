-- SCRIPT TO CREATE HOSPITAL AND CLINICS IN SAIGON

INSERT INTO Hospital
	(
		Hospital_Name,
		Hospital_Type,
		[Address],
		Ward_ID,
		District_ID,
		City_ID,
		Phone_Number,
		Fax,
		Email,
		Website,
		Coordinate,
		Created_Person,
		Is_Active
	)
VALUES
	-- INTERNATIONAL HOSPITALS - MULTI SPECILITIES
	(N'SOS International', 3, N'167A Nam Kỳ Khởi Nghĩa, Quận 3, Tp.Hồ Chí Minh',
	27124, 770, 79, '(08) 38298520', '(08) 38298551',
	'1hcm.ops@internationalsos.com', 'www.internationalsos.com',
	'10.784075, 106.689859', 1, 'True'),

	(N'HCM Family Medical Practice', 7, N'34 Diamond Plaza Lê Duẩn, Quận 3, Tp.Hồ Chí Minh',
	26740, 760, 79, '(08) 38227848', '(08) 38227859',
	'hcmc@vietnammedicalpractice.com', 'www.vietnammedicalpractice.com',
	'10.781278, 106.698689', 1, 'True'),

	(N'HCM Family Medical Practice', 7, N'95 Thảo Điền, Quận 2, Tp.Hồ Chí Minh',
	27088, 769, 79, '(08) 37442000', '(08) 38227859',
	'hcmc@vietnammedicalpractice.com', 'www.vietnammedicalpractice.com',
	'10.807854, 106.733785', 1, 'True'),

	(N'Bệnh viện đa khoa quốc tế Vũ Anh', 7, N'15-16 Phan Văn Trị, Phường 7, Quận Gò Vấp',
	26890, 764, 79, '(08) 39894989', NULL,
	'vuanhhospital@hotmail.com', 'www.benhvienvuanh.com.vn',
	'10.828697, 106.685580', 1, 'True'),

	(N'Bệnh viện Pháp Việt', 7, N'6 Nguyễn Lương Bằng, Phường Tân Phú, Quận 7',
	27487, 778, 79, '(08) 54113500', NULL,
	'information@fvhospital.com', 'www.fvhospital.com',
	'10.732597, 106.718450', 1, 'True'),

	(N'Phòng khám Pháp Việt', 7, N'Lầu 2 tòa nhà Citilight 45 Võ Thị Sáu, Phường Đakao, Quận 1',
	26737, 760, 79, '(08) 62906167', '(08) 22096168',
	'saigonclinic@fvhospital.com', NULL,
	'10.790250, 106.694659', 1, 'True'),

	(N'Phòng khám Columbia Asia', 3, N'8 Alexandre De Rhodes, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 38238888', '(08) 38238454',
	'saigonclinic@colombiaasia.com', 'www.colombiaasia.com',
	'10.779514, 106.696655', 1, 'True'),

	(N'Bệnh viện quốc tế Columbia Asia Gia Định', 7, N'1 Nơ Trang Long, Phường 7, Quận Bình Thạnh',
	26926, 765, 79, '(08) 38030677 / 38030678', NULL,
	'giadinhhospital@colombiaasia.com', NULL,
	'10.802870, 106.694922', 1, 'True'),

	(N'Phòng khám đa khoa quốc tế CMI', 3, N'1 Hàn Thuyên, Phường Bến Thành, Quận 1',
	 26743, 760, 79, '(08) 38272366 / 38272367', '(08)38272365',
	 'info@cmi-vietnam.com', 'www.cmi-vietnam.com', 
	 '10.779533, 106.698693', 1, 'True'),

	(N'Bệnh viện quốc tế Victoria Healthcare', 7, N'79 Điện Biên Phủ, Phường Đakao, Quận 1',
	26737, 760, 79, '(08) 39104545', '(08) 39103334',
	NULL, 'www.victoriavn.com',
	'10.789735, 106.697051', 1, 'True'),

	(N'Bệnh viện quốc tế Victoria Healthcare', 7, N'135A Nguyễn Văn Trỗi, Phường 12, Quận Phú Nhuận',
	27082, 768, 79, '(08) 39104545', '(08) 39103334',
	NULL, 'www.victoriavn.com',
	'10.794288, 106.677600', 1, 'True'),

	(N'Bệnh viện của tổ chức di trú quốc tế', 7, N'1B Phạm Ngọc Thạch, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 38222057', '(08) 38221780',
	'1hcm.ops@internationalsos.com', 'www.internationalsos.com',
	'10.781206, 106.697611', 1, 'True'),

	(N'Phòng khám đa khoa SAIGON - ITO', 3, N'800 Trần Hưng Đạo, Phường 7, Quận 5',
	 27322, 774, 79, '(08) 39225995 / 39225979', NULL,
	 'saigonito123@gmail.com', 'www.saigonitohospital.com', 
	 '10.753821, 106.676066', 1, 'True'),

	-- LOCAL HOSPITALS - MULTI SPECILITIES
	(N'Bệnh viện Chợ Rẫy', 1, N'205 Nguyễn Chí Thanh, Phường Bến Nghé, Phường 12, Quận 5',
	27310, 774, 79, '(08) 38554137', '(08) 38557267',
	'bvchoray@hcm.vnn.vn', 'www.choray.vn',
	'10.757784, 106.659102', 1, 'True'),

	(N'Bệnh viện đa khoa Sài Gòn', 1, N'125 Lê Lợi, Phường Bến Thành, Quận 1',
	26743, 760, 79, '(08) 3829 2071 / 38291711', NULL,
	'bv.saigon@tphcm.gov.vn', NULL,
	'10.772018, 106.699451', 1, 'True'),

	(N'Bệnh viện An Bình', 1, N'146 An Bình, Phường 7, Quận 5',
	27322, 774, 79, '(08) 39234260', NULL,
	NULL, NULL,
	'10.753992, 106.672045', 1, 'True'), 

	(N'Bệnh viện Thống Nhất', 1, N'1 Lý Thường Kiệt, Phường 7, Quận Tân Bình',
	26986, 766, 79, '(08) 3862141', NULL,
	NULL, NULL,
	'10.790774, 106.653135', 1, 'True'),

	(N'Bệnh viện Nguyễn Trãi', 1, N'314 Nguyễn Trãi, Phường 8, Quận 5',
	27316, 774, 79, '(08) 3923592', '(08) 38382182',
	NULL, NULL,
	'10.756006, 106.675087', 1, 'True'),

	(N'Bệnh viện Nguyễn Tri Phương', 1, N'468 Nguyễn Trãi, Phường 8, Quận 5',
	27316, 774, 79, '(08) 39234332', '(08) 39236858',
	NULL, NULL,
	'10.754773, 106.670034', 1, 'True'),

	(N'Bệnh viện Nhân Dân Gia Định', 1, N'1 Nơ Trang Long, Phường 7, Quận Bình Thạnh',
	26926, 765, 79, '(08) 38412697', '(08) 48412700',
	NULL, NULL,
	'10.803071, 106.694205', 1, 'True'), 

	(N'Bệnh viện Bình Dân', 1, N'371 Điện Biên Phủ, Phường 4, Quận 3',
	27148, 770, 79, '(08) 38394747', '(08) 38391315',
	NULL, NULL,
	'10.774559, 106.681403', 1, 'True'),

	(N'Bệnh viện Nhân Dân 115', 1, N'527 Sư Vạn Hạnh, Phường 12, Quận 10',
	27172, 771, 79, '(08) 38683496', '(08) 38655193',
	NULL, NULL,
	'10.775143, 106.667568', 1, 'True'),

	(N'Bệnh viện 30/4', 1, N'9 Sư Vạn Hạnh, Phường 9, Quận 5',
	27304, 774, 79, '(08) 38354986', '(08) 38399170',
	NULL, NULL,
	'10.758950, 106.672897', 1, 'True'),

	(N'Bệnh viện Triều An', 2, N'425 Kinh Dương Vương, Phường An Lạc, Quận Bình Tân',
	27460, 777, 79, '(08) 37508888 / 37509999', '(08) 37510915',
	NULL, NULL,
	'10.739325, 106.616660', 1, 'True'),

	(N'Bệnh viện Hoàn Mỹ', 2, N'60-60A Phan Xích Long, Phường 1, Quận 3',
	27160, 770, 79, '(08) 39902468', '(08) 39311940',
	NULL, NULL,
	'10.800342, 106.684290', 1, 'True'),

	(N'Bệnh viện An Sinh', 1, N'10 Trần Huy Liệu, Phường 12, Quận Phú Nhuận',
	27082, 768, 79, '(08) 38457777', '(08) 38476734',
	'info@ansinh.com.vn', 'www.ansinh.com.vn',
	'10.791235, 106.678548', 1, 'True'),

	(N'Bệnh viện Đại học Y Dược', 1, N'215 Hồng Bàng, Phường 11, Quận 5',
	27328, 774, 79, '(08) 3854269', '(08) 39506126',
	NULL, NULL,
	'10.755203, 106.664358', 1, 'True'),

	(N'Bệnh viện đa khoa Hồng Đức', 1, N'32/2 Thống Nhất, Phường 10, Quận Gò Vấp',
	26884, 764, 79, '(08) 39893594', '(08) 38959612',
	'info@hongduchspital.vn', NULL,
	'10.833148, 106.663932', 1, 'True'),

	(N'Bệnh viện đa khoa Hồng Đức', 1, N'234 Pasteur, Phường 6, Quận 3',
	27139, 770, 79, '(08) 39893594', '(08) 38959612',
	'info@hongduchspital.vn', NULL,
	'10.785087, 106.690963', 1, 'True'),

	(N'Bệnh viện Nhiệt Đới', 1, N'764 Võ Văn Kiệt, Phường 13, Quận 5',
	26896, 774, 79, '(08) 39235804', '(08) 39236943',
	'ttbnd@hcm.vnn.vn', 'www.bvdnd.vn',
	'10.752735, 106.678281', 1, 'True'),

	-- SPECIALTY HOSPITALS
	-- PEDIATRICS (NHI KHOA)
	(N'Bệnh viện Nhi Đồng 1', 1, N'374 Sư Vạn Hạnh, Phường 10, Quận 10',
	27178, 771, 79, '(08) 39271119', NULL,
	NULL, NULL,
	'10.769491, 106.671021', 1, 'True'),

	(N'Bệnh viện Nhi Đồng 2', 1, N'14 Lý Tự Trọng, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 38298385', NULL,
	NULL, NULL,
	'10.780565, 106.703205', 1, 'True'),

	-- BLOOD AND DIANOSTICS TEST (HUYẾT HỌC VÀ XÉT NGHIỆM)
	(N'Viện Pasteur', 1, N'167 Pasteur, Phường 8, Quận 3',
	27121, 770, 79, '(08) 38202835', '(08) 38231419',
	NULL, NULL,
	'10.786088, 106.688047', 1, 'True'), 

	(N'Bệnh viện Truyền Máu Huyết Học', 1, N'152 Hồng Bàng, Phường 12, Quận 5',
	27310, 774, 79, '(08) 39557858', '(08) 38552978',
	'hemato@vnn.vn', NULL,
	'10.767515, 106.684456', 1, 'True'),

	(N'Trung tâm chẩn đoán Y khoa Medic', 4, N'254 Hòa Hảo, Phường 4, Quận 10',
	27310, 774, 79, '(08) 39557858', '(08) 38552978',
	'hemato@vnn.vn', NULL,
	'10.762655, 106.670095', 1, 'True'),

	-- CARDIOLOGY (BỆNH TIM)
	(N'Viện tim thành phố Hồ Chí Minh', 1, N'88 Thành Thái, Phường 12, Quận 10',
	27172, 771, 79, '(08) 38651586', '(08) 38654026',
	NULL, NULL,
	'10.773235, 106.666956', 1, 'True'),

	(N'Viện tim Tâm Đức', 1, N'4 Nguyễn Lương Bằng, Phường Tân Phú, Quận 7',
	27487, 778, 79, '(08) 54110036 / 54110025', '(08) 54110029',
	NULL, 'www.tamduchearthospital.com',
	'10.733386, 106.717844', 1, 'True'),

	-- TRAUMA HOSPITAL / TRAMATOLOGY & ORTHOPAEDICS (CHẤN THƯƠNG CHỈNH HÌNH)
	(N'Bệnh viện Chấn Thương Chỉnh Hình', 1, N'929 Trần Hưng Đạo, Phường 1, Quận 5',
	27325, 774, 79, '(08) 39237007', '(08) 39236554',
	NULL, 'www.tamduchearthospital.com',
	'10.754445, 106.678281', 1, 'True'),

	(N'Bệnh viện Chấn Thương Chỉnh Hình SAIGON - ITO', 1, N'305 Lê Văn Sỹ, Phường 1, Quận Tân Bình',
	26977, 766, 79, '(08) 38441399 / 39912029', '(08) 39236554',
	'saigon-ito@hcm.vnn.vn', 'saigonitohospital.com',
	'10.796768, 106.664768', 1, 'True'),

	-- MENTAL ILLNESS (THẦN KINH)
	(N'Bệnh viện Ngoại Thần Kinh Quốc Tế', 2, N'65 Lũy Bán Bích, Phường Tân Thới Hoà, Quận Tân Phú',
	27040, 767, 79, '(08) 39616996', '(08) 384441399',
	NULL, NULL,
	'10.761397, 106.632411', 1, 'True'),

	(N'Bệnh viện tâm thần thành phố Hồ Chí Minh', 2, N'766 Võ Văn Kiệt, Phường 1, Quận 5',
	27325, 774, 79, '(08) 39234675', '(08) 39234880',
	NULL, 'www.bvtt-tphcm.org.vn',
	'10.746131, 106.653795', 1, 'True'),

	-- TURBERCULOSIS HOSPITAL (BỆNH LAO)
	(N'Bệnh viện Phạm Ngọc Thạch', 1, N'120 Hùng Vương, Phường 12, Quận 5',
	27310, 774, 79, '(08) 38550207', '(08) 38574264',
	NULL, NULL,
	'10.756197, 106.665195', 1, 'True'),

	-- OBSTETRICS & GENAECOLOGY(SẢN KHOA - DI TRUYỀN)
	(N'Bệnh viện Phụ Sản Quốc Tế Sài Gòn', 7, N'63 Bùi Thị Xuân, Phường Phạm Ngũ Lão, Quận 1',
	26749, 760, 79, '(08) 39253623', '(08) 38574264',
	'sihospital@hcm.vnn.vn', 'www.sihospital.com.vn',
	'10.769878, 106.688576', 1, 'True'),

	(N'Bệnh viện Từ Dũ', 1, N'284 Cống Quỳnh, Phường Phạm Ngũ Lão, Quận 1',
	26749, 760, 79, '(08) 38392722', '(08) 38396832',
	NULL, NULL,
	'10.768782, 106.685560', 1, 'True'),

	(N'Bệnh viện Hùng Vương', 1, N'128 Hồng Bàng, Phường 12, Quận 5',
	27310, 774, 79, '(08) 38551835', '(08) 38574365',
	NULL, NULL,
	'10.756425, 106.661917', 1, 'True'),

	(N'Phòng khám Bệnh viện Quốc tế HẠNH PHÚC', 3, N'Lầu 2 tòa nhà Trung tâm thương mại Sài Gòn 37 Tôn Đức Thắng, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 39111860', '(08) 39111861',
	'www.hanhphuchospital.com', NULL,
	'10.784017, 106.703627', 1, 'True'),

	(N'Bệnh viện Hạnh Phúc', 7, N'13 Vĩnh Phú, Thị Xã Thuận An',
	27310, 725, 74, '(650) 39111860', '(650) 3636069',
	'www.hanhphuchospital.com', 'info@hanhphuchospital.com',
	'10.756425, 106.661917', 1, 'True'),

	(N'Trung tâm Chăm sóc Sức khỏe Quốc tế HẠNH PHÚC', 4, N'97 Nguyễn Thị Minh Khai, Phường Bến Thành, Quận 1',
	26743, 760, 79, '(08) 39259797', '(08) 39259949',
	'www.hanhphuchospital.com', 'saigonclinic@hanhphuchospital.com',
	'10.774563, 106.690308', 1, 'True'),

	-- ONCOLOGY (UNG THƯ)
	(N'Bệnh viện Ung Bướu', 1, N'3 Nơ Trang Long, Phường 7, Quận Bình Thạnh',
	26926, 765, 79, '(08) 38433021', '(08) 38412636',
	'bvubhcm@gmail.com', NULL,
	'10.805017, 106.694091', 1, 'True'),

	-- ACUPUNCTURE(CHÂM CỨU)
	(N'Bệnh viện Y học cổ tryền', 1, N'179 Nam Kỳ Khởi Nghĩa, Phường 7, Quận 3',
	27124, 770, 79, '(08) 39326579', NULL,
	NULL, NULL,
	'10.786036, 106.687739', 1, 'True'),

	-- SKIN TREATMENT (BỆNH NGOÀI DA)
	(N'Bệnh viện da liễu', 1, N'2 Nguyễn Thông, Phường 6, Quận 3',
	27139, 770, 79, '(08) 39305233', '(08) 39304810',
	NULL, NULL,
	'10.776437, 106.686602', 1, 'True'),

	(N'Stamford Skin Clinic', 1, N'99 Sương Nguyệt Ánh Phường Bến Thành, Quận 1',
	26743, 760, 79, '(08) 39251990', NULL,
	NULL, 'stamfordskin.com',
	'10.771251, 106.688506', 1, 'True'),

	-- DENTAL CARE (NHA KHOA)
	(N'Bệnh viện Răng Hàm Mặt', 1, N'263-264 Trần Hưng Đạo, Phường Cô Giang, Quận 1',
	26755, 760, 79, '(08) 38360191', '(08) 38367319',
	'bvranghammat@vnn.vn', 'www.bvranghammat.com',
	'10.763476, 106.691719', 1, 'True'),

	(N'Nha Khoa Koseikai', 3, N'21 Nguyễn Trung Ngạn, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 39106255', '(08) 39106256',
	'koseikaivietnam@hcm.vnn.vn', 'www.hcpg.jp',
	'10.782974, 106.704898', 1, 'True'),

	(N'Grand Dentistry', 3, N'32-34 Ngô Đức Kế, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 38219446', '(08) 39104018',
	'granddentist@yahoo.com', NULL,
	'10.773357, 106.70535', 1, 'True'),

	(N'Maple Health Care', 3, N'72 Võ Thị Sáu, Phường Đa Kao, Quận 1',
	26737, 760, 79, '(08) 38201999', '(08) 38204619',
	NULL, 'www.maplehealthcare.net',
	'10.789092, 106.693103', 1, 'True'),

	(N'Nha khoa Starlight', 3, N'2 Bis Công trường Quốc Tế, Phường 6, Quận 3',
	27139, 770, 79, '(08) 38226222 / 38239294', '(08) 38239275',
	'starlightdental@gmail.com', 'www.starlightdental.net',
	'10.782699, 106.696305', 1, 'True'),

	(N'Nha khoa Viễn Đông', 3, N'249 Lê Thánh Tôn, Phường Bến Thành, Quận 1',
	26743, 760, 79, '(08) 822448888 / 38233405', '(08) 38257789',
	'info@fareastdental.com', 'www.fareastdental.com',
	'10.782699, 106.696305', 1, 'True'),
		
	(N'Nha khoa Quốc tế Westcoast Bến Thành', 3, N'27 Nguyễn Trung Trực, Phường Bến Thành, Quận 1',
	26743, 760, 79, '(08) 38256999', '(08) 38257485',
	'benthanh@westcoastinternational.com', 'www.westcoastinternational.com',
	'10.774564, 106.698468', 1, 'True'),

	(N'Nha khoa Quốc tế Westcoast Đồng Khởi', 3, N'71-79 Đồng Khởi, Phường Bến Nghé, Quận 1',
	26740, 760, 79, '(08) 38256777', '(08) 38257838',
	'dongkhoi@westcoastinternational.com', 'www.westcoastinternational.com',
	'10.774712, 106.70453', 1, 'True'),
	
	(N'Khoa Răng Hàm Mặt - Đại Học Y Dược TP.HCM', 1, N'652 Nguyễn Trãi, Phường 11, Quận 5',
	27328, 774, 79, '(08) 38558735 / 38552641', '(08) 38552300',
	NULL, 'www.bvdaihoc.com.vn',
	'10.753483, 106.663706', 1, 'True'),

	(N'Nha khoa Quốc tế German', 3, N'Đảo Kim Cương, Phường Bình Trưng Tây, Quận 2',
	27100, 769, 79, '(08) 35001636', NULL,
	'info@gid-dentistry.com', 'www.gid-dentistry.com',
	'10.778752, 106.746685', 1, 'True'),

	-- OPTHTHALMOLOGY (BỆNH VIỆN MẮT)
	(N'Bệnh viện Mắt', 1, N'280 Điện Biên Phủ, Phường 7, Quận 3',
	27124, 770, 79, '(08) 39326732', '(08) 39325713',
	'hochiminhcityeyehospital@yahoo.com', NULL,
	'10.778409, 106.685325', 1, 'True'), 

	(N'Bệnh viện Mắt Sài Gòn', 1, N'100 Lê Thị Riêng, Phường Bến Thành, Quận 1',
	26743, 760, 79, '(08) 39256155 / 39256158', NULL,
	NULL, 'www.matsaigon.com',
	'10.771174, 106.691099', 1, 'True'),

	(N'Phòng khám mắt Cao Thắng', 3, N'135 Trần Bình Trọng, Phường 2, Quận 5',
	27313, 774, 79, '(08) 39238435', '38384464',
	'info@cthospital.vn', 'www.cthospital.vn',
	'10.757510, 106.681045', 1, 'True'),
	
	(N'Phòng khám chuyên khoa mắt - American Eye Center', 3, N'Lầu 5 Crescent Plaza, 105 Tôn Dật Tiên, Phường Tân Phong, Quận 7',
	27490, 778, 79, '(08) 54136758 / 54136759', '(08) 54136760',
	'info@americaneyecentervn.com', 'www.americaneyecentervn.com',
	'10.722882, 106.71425', 1, 'True'),

	(N'Bệnh viện mắt Việt Hàn', 1, N'355 Ngô Gia Tự, Phường 3, Quận 10',
	27205, 771, 79, '(08) 38300999', NULL,
	NULL, 'www.matsaigon.com',
	'10.761315, 106.670058', 1, 'True'),

	-- OTORHINOLARYNGOLOGY HOSPITAL (TAI MŨI HỌNG)
	(N'Bệnh viện Tai Mũi Họng', 1, N'155B Trần Quốc Thảo, Phường 9, Quận 3',
	27142, 770, 79, '(08) 39317381', '(08) 39312712',
	'entcenter@hcm.fpt.vn', NULL,
	'10.784459, 106.683985', 1, 'True')