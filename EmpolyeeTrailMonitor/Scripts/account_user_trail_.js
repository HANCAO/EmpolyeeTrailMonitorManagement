var farmer_baidu_ = {
    user_id: '',
    user_name: '',
    mileage: '',
    car_mileage: '',
    address: '',
    reason: '',
    farmerList: [],
    
    searchLocationHYERP: function (bill_no) {
       
        // 百度地图API功能
        $.ajax({
            url: 'ReadTableList',
            type: 'post',
            //data: { user_id: farmer_baidu_.user_id, car_bill_no: bill_no },
            dataType: "json",
            success: function (value) {
                console.log(value);


                if (farmer_baidu_.Location_Map) {

                    var points = [];
                    var datas = [];
                    for (var i = 0; i < value.rows.length; i++) {

                        farmer_baidu_.orgcommonMap(farmer_baidu_.Location_Map, value, i);
                    }

                    farmer_baidu_.Location_Map.centerAndZoom(new BMap.Point(value.rows[0].GPS_X, value.rows[0].GPS_Y), 11);
                    farmer_baidu_.Location_Map.setCenter(new BMap.Point(value.rows[0].GPS_X, value.rows[0].GPS_Y))
                    farmer_baidu_.Location_Map.setZoom(15);
                }
                else {
                    farmer_baidu_.farmerMap();
                    if (farmer_baidu_.Location_Map) {
                        for (var i = 0; i < value.rows.length; i++) {
                            farmer_baidu_.orgcommonMap(farmer_baidu_.Location_Map, value, i);

                        }
                        farmer_baidu_.Location_Map.centerAndZoom(new BMap.Point(value.rows[0].GPS_X, value.rows[0].GPS_Y), 11);
                        farmer_baidu_.Location_Map.setCenter(new BMap.Point(value.rows[0].GPS_X, value.rows[0].GPS_Y))
                        farmer_baidu_.Location_Map.setZoom(15);
                    }
                    else {

                    }
                }

                farmer_baidu_.DrawTrajectory();

            }
        })
    },
    bill_no: '',
    create_date: '',
    Location_Map: null,
    DrawTrajectory: function () {
        farmer_baidu_.ajaxLoading("#TrackMap", ($(window).outerWidth(true) - 300) / 2, ($(window).height() - 150) / 2);
        // 百度地图API功能
        $.ajax({
            url: 'getUserTrail',
            type: 'post',
            data: { bill_no: farmer_baidu_.bill_no, start_time: $("#start_time").timespinner("getValue"), end_time: $("#end_time").timespinner("getValue"), distance: $("#distance").textbox("getValue") },
            dataType: "json",
            success: function (value) {
                farmer_baidu_.ajaxLoadEnd();
                if (value.rows.length > 0) {
                    var pointGCJ = [];
                    var pointBD = [];
                    for (var i = 0; i < value.rows.length; i++) {
                        pointGCJ = wgs84togcj02(value.rows[i].GPS_X, value.rows[i].GPS_Y);
                        pointBD = gcj02tobd09(pointGCJ[0], pointGCJ[1]);
                        value.rows[i].Bmap_lng = pointBD[0];
                        value.rows[i].Bmap_lat = pointBD[1];
                    }
                    if (farmer_baidu_.Location_Map) {
                        var pointList = [];
                        for (var i = 0; i < value.rows.length; i = i + 1) {
                            farmer_baidu_.commonMap(farmer_baidu_.Location_Map, value, i);
                            pointList.push(new BMap.Point(value.rows[i].Bmap_lng, value.rows[i].Bmap_lat));
                        }
                        var polyline = new BMap.Polyline(pointList, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });   //创建折线
                        farmer_baidu_.Location_Map.addOverlay(polyline);   //增加折线
                        farmer_baidu_.Location_Map.setCenter(new BMap.Point(value.rows[0].Bmap_lng, value.rows[0].Bmap_lat))
                        farmer_baidu_.Location_Map.setZoom(20);


                       
                    }
                    else {
                        farmer_baidu_.farmerMap();
                        if (farmer_baidu_.Location_Map) {
                            var pointList = [];
                            for (var i = 0; i < value.rows.length; i = i + 1) {
                                farmer_baidu_.commonMap(farmer_baidu_.Location_Map, value, i);
                                pointList.push(new BMap.Point(value.rows[i].Bmap_lng, value.rows[i].Bmap_lat));

                            }

                            var polyline = new BMap.Polyline(pointList, { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });   //创建折线
                            farmer_baidu_.Location_Map.addOverlay(polyline);   //增加折线

                            farmer_baidu_.Location_Map.setCenter(new BMap.Point(value.rows[0].Bmap_lng, value.rows[0].Bmap_lat))
                            farmer_baidu_.Location_Map.setZoom(20);
                        }
                        else {
                           
                        }
                    }

                }
                else {
                   
                }
            }
        })
    },
    commonMap: function (map, value, i) {
        if (value.rows[i].Bmap_lng != null && value.rows[i].Bmap_lat != null) {
            var point = new BMap.Point(value.rows[i].Bmap_lng, value.rows[i].Bmap_lat);
            map.centerAndZoom(point, 11);
            var opts = {
                position: point,    // 指定文本标注所在的地理位置
                offset: new BMap.Size(10, 25)    //设置文本偏移量
            }
            var label = new BMap.Label(commonTool.dateTimeConvert(value.rows[i].create_time, "HH:mm:ss"), opts);  // 创建文本标注对象
            var marker = new BMap.Marker(point);

            //var size = new BMap.Size(16, 16);
            //var myIcon = new BMap.Icon("http://api.map.baidu.com/img/markers.png", new BMap.Size(23, 25), {
            //    offset: new BMap.Size(10, 25), // 指定定位位置  
            //    imageOffset: new BMap.Size(0, 0 - 10 * 30) // 设置图片偏移  
            //});

            marker.setLabel(label);
            //marker.setIcon(myIcon);
            map.addOverlay(marker);
            label.setStyle({
                borderColor: "#808080",
                color: "#333",
                cursor: "pointer"
            });

        }
    },

    orgcommonMap: function (map, value, i) {

        if (value.rows[i].GPS_X != null && value.rows[i].GPS_Y != null) {
            var point = new BMap.Point(value.rows[i].GPS_X, value.rows[i].GPS_Y);

            var opts = {
                position: point,    // 指定文本标注所在的地理位置
                offset: new BMap.Size(10, 25)    //设置文本偏移量
            }
            var label = new BMap.Label(value.rows[i].name, opts);  // 创建文本标注对象
            var marker = new BMap.Marker(point);
            var size = new BMap.Size(16, 16);
            var myIcon = new BMap.Icon("http://api.map.baidu.com/img/markers.png", new BMap.Size(23, 25), {
                offset: new BMap.Size(10, 25), // 指定定位位置  
                imageOffset: new BMap.Size(0, 0 - 10 * 30) // 设置图片偏移  
            });

            marker.setLabel(label);
            marker.setIcon(myIcon);
            map.addOverlay(marker);
            label.setStyle({
                borderColor: "#808080",
                color: "#333",
                cursor: "pointer"
            });
            (function () {
                var index = i;
                var _iw = farmer_baidu_.createInfoWindow(value.rows[i]);
                var _marker = marker;
                _marker.addEventListener("click", function () {
                    this.openInfoWindow(_iw);
                });
                _iw.addEventListener("open", function () {
                    _marker.getLabel().hide();
                })
                _iw.addEventListener("close", function () {
                    _marker.getLabel().show();
                })
                label.addEventListener("click", function () {
                    _marker.openInfoWindow(_iw);
                })
                if (!!value.rows[i].isOpen) {
                    label.hide();
                    _marker.openInfoWindow(_iw);
                }
            })()
        }
    },
    farmerMap: function (x, y) {
        x = 0 || x;
        y = 0 || y;
        var farmerMapHYERP = new BMap.Map("TrackMap");
        farmerMapHYERP.addControl(new BMap.NavigationControl());               // 添加平移缩放控件
        farmerMapHYERP.addControl(new BMap.ScaleControl());                    // 添加比例尺控件
        farmerMapHYERP.addControl(new BMap.OverviewMapControl());
        farmerMapHYERP.addControl(new BMap.MapTypeControl());
        farmerMapHYERP.enableScrollWheelZoom(true);  //添加滚轮缩放 
        farmerMapHYERP.enableDragging(); //启用地图拖拽事件，默认启用(可不写)
        farmerMapHYERP.enableDoubleClickZoom(); //启用鼠标双击放大，默认启用(可不写)
        farmerMapHYERP.enableKeyboard(); //启用键盘上下左右键移动地图
        //farmerMapHYERP.centerAndZoom(new BMap.Point(value.rows[0].Bmap_lng, value.rows[0].Bmap_lat), 11);  // 初始化地图
        farmer_baidu_.Location_Map = farmerMapHYERP;
        farmerMapHYERP.setCurrentCity("北京");
        farmerMapHYERP.centerAndZoom(new BMap.Point(x, y), 11);  // 初始化地图
        //farmer_baidu_.DrawTrajectory();
    },
    //创建InfoWindow
    createInfoWindow: function (i) {
        var json = i;
        var iw = new BMap.InfoWindow("<b class='iw_poi_title' title='" + json.name + "'>" + json.name + "</b><div class='iw_poi_content'>" + json.map_info + "</div>");
        return iw;
    }
}
$(function () {
    farmer_baidu_.searchLocationHYERP();
    //account_user_trail_.init();
});
/**
 * WGS84转GCj02
 * @param lng
 * @param lat
 * @returns {*[]}
 */
var wgs84togcj02 = function wgs84togcj02(lng, lat) {
    var lat = +lat;
    var lng = +lng;
    if (out_of_china(lng, lat)) {
        return [lng, lat]
    } else {
        var dlat = transformlat(lng - 105.0, lat - 35.0);
        var dlng = transformlng(lng - 105.0, lat - 35.0);
        var radlat = lat / 180.0 * PI;
        var magic = Math.sin(radlat);
        magic = 1 - ee * magic * magic;
        var sqrtmagic = Math.sqrt(magic);
        dlat = (dlat * 180.0) / ((a * (1 - ee)) / (magic * sqrtmagic) * PI);
        dlng = (dlng * 180.0) / (a / sqrtmagic * Math.cos(radlat) * PI);
        var mglat = lat + dlat;
        var mglng = lng + dlng;
        return [mglng, mglat]
    }
};

/**
 * GCJ02 转换为 WGS84
 * @param lng
 * @param lat
 * @returns {*[]}
 */
var gcj02tobd09 = function gcj02tobd09(lng, lat) {
    var lat = +lat;
    var lng = +lng;
    var z = Math.sqrt(lng * lng + lat * lat) + 0.00002 * Math.sin(lat * x_PI);
    var theta = Math.atan2(lat, lng) + 0.000003 * Math.cos(lng * x_PI);
    var bd_lng = z * Math.cos(theta) + 0.0065;
    var bd_lat = z * Math.sin(theta) + 0.006;
    return [bd_lng, bd_lat]
};
/**
 * 判断是否在国内，不在国内则不做偏移
 * @param lng
 * @param lat
 * @returns {boolean}
 */
var out_of_china = function out_of_china(lng, lat) {
    var lat = +lat;
    var lng = +lng;
    // 纬度3.86~53.55,经度73.66~135.05 
    return !(lng > 73.66 && lng < 135.05 && lat > 3.86 && lat < 53.55);
};

var transformlat = function transformlat(lng, lat) {
    var lat = +lat;
    var lng = +lng;
    var ret = -100.0 + 2.0 * lng + 3.0 * lat + 0.2 * lat * lat + 0.1 * lng * lat + 0.2 * Math.sqrt(Math.abs(lng));
    ret += (20.0 * Math.sin(6.0 * lng * PI) + 20.0 * Math.sin(2.0 * lng * PI)) * 2.0 / 3.0;
    ret += (20.0 * Math.sin(lat * PI) + 40.0 * Math.sin(lat / 3.0 * PI)) * 2.0 / 3.0;
    ret += (160.0 * Math.sin(lat / 12.0 * PI) + 320 * Math.sin(lat * PI / 30.0)) * 2.0 / 3.0;
    return ret
};

var transformlng = function transformlng(lng, lat) {
    var lat = +lat;
    var lng = +lng;
    var ret = 300.0 + lng + 2.0 * lat + 0.1 * lng * lng + 0.1 * lng * lat + 0.1 * Math.sqrt(Math.abs(lng));
    ret += (20.0 * Math.sin(6.0 * lng * PI) + 20.0 * Math.sin(2.0 * lng * PI)) * 2.0 / 3.0;
    ret += (20.0 * Math.sin(lng * PI) + 40.0 * Math.sin(lng / 3.0 * PI)) * 2.0 / 3.0;
    ret += (150.0 * Math.sin(lng / 12.0 * PI) + 300.0 * Math.sin(lng / 30.0 * PI)) * 2.0 / 3.0;
    return ret
};


//定义一些常量
var x_PI = 3.14159265358979324 * 3000.0 / 180.0;
var PI = 3.1415926535897932384626;
var a = 6378245.0;
var ee = 0.00669342162296594323;
/**
 * Created by Wandergis on 2015/7/8. 使用小程序里的coordtransform.js
 * 提供了百度坐标（BD09）、国测局坐标（火星坐标，GCJ02）、和WGS84坐标系之间的转换
 */
//UMD魔法代码
// if the module has no dependencies, the above pattern can be simplified to
