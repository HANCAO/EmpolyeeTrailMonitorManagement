--显示原表所有用户
SELECT DISTINCT create_user FROM [dbo].[account_user_trail];

--显示原表8888用户的行径数据
SELECT * FROM account_user_trail WHERE create_user=8888;

--查看两表所有数据
SELECT * FROM EmpolyeeTrail;
SELECT * FROM account_user_trail WHERE Bmap_lat IS NULL;

--将原表中的全部数据导入到新表中
SET IDENTITY_INSERT EmpolyeeTrail ON;

INSERT INTO EmpolyeeTrail(Id,GPSX,GPSY,BmapLap,BmapLng,CreateTime,CreateUser,IsCar,Distance,DistanceSecond) SELECT id,GPS_X,GPS_Y,Bmap_lat,Bmap_lng,create_time,create_user,is_car,distance,distance_second FROM account_user_trail WHERE Bmap_lat IS NOT NULL OR Bmap_lng IS NOT NULL OR distance IS NOT NULL OR distance_second IS NOT NULL;

INSERT INTO EmpolyeeTrail(Id,GPSX,GPSY,BmapLap,BmapLng,CreateTime,CreateUser,IsCar,Distance,DistanceSecond) SELECT id,GPS_X,GPS_Y,Bmap_lat,Bmap_lng,create_time,create_user,is_car,distance,distance_second FROM account_user_trail WHERE create_user=8888;

--日期时间数据相减（以小时为单位）
SELECT DATEDIFF(HOUR, '2018-1-1 10:20:45', '2018-1-24 22:28:45') AS '相距时间';
select datediff( second, '2018/10/13 18:27:39', '2018/10/13 18:28:44') 

--删除新表中所有数据
DELETE FROM EmpolyeeTrail;

--视图组件查看共有多少条数据和用户
SELECT COUNT(*) FROM EmpolyeeTrail;
SELECT COUNT(DISTINCT CreateUser) FROM EmpolyeeTrail;

--查看索引
exec sp_helpindex EmpolyeeTrail;
exec sp_helpindex account_user_trail;

--创建索引
create index create_user_index on EmpolyeeTrail (CreateUser);
