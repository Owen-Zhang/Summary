select 
		a.id, 
		a.station_id, 
		a.oil_no, 
		a.listing_customize_type, 
		a.listing_now_calc_type, -- 挂牌现价计算方式（1.挂牌原价销售，2.活动挂牌直降）
		a.listing_orig_price, -- 挂牌原价（元/升）
		a.listing_now_price, -- 挂牌现价（元/升）
		a.last_update_time,
		a.activity_schedule_id, -- 活动标识
		s.begin_time as activity_begin_time, -- 活动开始时间
		s.end_time as activity_end_time,  -- 活动结束时间
		s.period_params as activity_period_params, -- 活动区间
		s.period_type as activity_period_type, -- 活动类型
		s.time_range as activity_time_range, -- 周的时间段
		s.schedule_name as activity_schedule_name, -- 活动名称
		s.price_params as activity_price_params -- 活动调价信息
	from crp_up_product_extend a 
	inner join (
		SELECT substring_index(GROUP_CONCAT(e.id ORDER BY e.station_id DESC),',',1) as id
		FROM crp_up_product_extend e
		INNER JOIN crp_station_info s ON s.up_channel_no = e.up_channel_no AND s.station_id = @station_id
		where (e.station_id = @station_id or e.station_id = 0)
		group by e.oil_no
	) b on a.id = b.id
