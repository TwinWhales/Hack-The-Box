/**
 * menuObject
 *  - 
 * author: Moa Chung
 * date:   2013-08-22
 * notice: 
 */
function menuObject() {
	this.v4v6;
	this.usb;
	var changes_count = '0';

	//main
	var basic_L1 =
		[
			{title:'_apply_discard',page:'adm_changes.asp?cat=0',style:'font: bold 16px Arial;'},
			{title:'_networkstate',page:'basic_status.asp',img:'but_network_status'},
			{title:'_wireless',page:'basic_wireless.asp',img:'but_wireless'},
			{title:'_guest_network',page:'basic_guest.asp',img:'but_guest_network'},
			{title:'_parental_control',page:'basic_parental_control.asp',img:'but_parental_control'},
			{title:'_router_limits',page:'basic_router_limits.asp',img:'but_router_limits'},
		];
	
	var advanced_L1 =
		[
			{title:'_apply_discard',page:'adm_changes.asp?cat=1',style:'font: bold 16px Arial;'},
			{title:'ADMIN',img:'but_administrator'},
			{title:'_setup',img:'but_setup'},
			{title:'_wireless_2',img:'but_wireless_24'},
			{title:'_wireless_5',img:'but_wireless_5'},
			{title:'ES_security',img:'but_security'},
			{title:'_firewall',img:'but_firewall'},
			{title:'_usb',img:'but_usb'}
		];
	var advanced_L2 = [
		[],
		//administrator(0)
		[
			{title:'_r_status_tew827dru',page:'adm_status.asp'},				{title:'_ipv6_status',page:'adm_ipv6status.asp'},
			{title:'_system_log',page:'adm_syslog.asp'},			{title:'_advnetwork',page:'adv_network.asp'},
			{title:'_settings_management',page:'adm_settings.asp'},		{title:'_time_cap',page:'adm_time.asp'},
			{title:'_dgnst',page:'adm_dgnst.asp'},		{title:'_c_status_tew827dru',page:'client_status.asp'}
		],
		//setup(1)
		[
			{title:'device_mode',page:'adm_opmode.asp'},			{title:'_lan_setting',page:'internet_lan.asp'},
			{title:'_wan_setting',page:'internet_wan.asp'},                 {title:'_routing',page:'adv_routing.asp'},
			{title:'_ipv6_setting',page:'internet_ipv6.asp'},		{title:'_sched',page:'adv_schedule.asp'},
			{title:'_upload_firm',page:'adm_upload_firmware.asp'},		{title:'_management',page:'adm_management.asp'},
			{title:'_wizard',page:'setup_wizard.asp'},			{title:'bd_DHCP',page:'internet_dhcpcliinfo.asp'},
			{title:'_openvpn_title',page:'openvpn.asp'},			{title:'_watchdog_title',page:'ping_watchdog.asp'},
			{title:'_reboot_title',page:'auto_reboot.asp'},			{title:'vlan_item_title',page:'adv_vlan.asp'}
		],
		//wireless 2.4GHz(2)
		[
			{title:'help743',page:'wireless_wds.asp'},			{title:'_advanced',page:'wireless_advanced.asp'},
			{title:'mult_ssid',page:'wireless_mssid.asp'},			/* {title:'ES_security',page:'wireless_security.asp'}, */
			{title:'_WPS',page:'wireless_wps.asp'},
		],
		//wireless 5GHz(3)
		[
			{title:'help743',page:'wireless2_wds.asp'},			{title:'_advanced',page:'wireless2_advanced.asp'},
			{title:'mult_ssid',page:'wireless2_mssid.asp'},			/* {title:'ES_security',page:'wireless2_security.asp'}, */
			{title:'_WPS',page:'wireless2_wps.asp'},
		],
		//security(4)
		[
			{title:'_acccon',page:'adv_access_control.asp'},		{title:'_inboundfilter',page:'adv_inbound_filter.asp'}
		],
		//firewall(5)
		[
			{title:'help488',page:'adv_dmz.asp'},					{title:'_virtserv',page:'adv_virtual.asp'},
			{title:'_specapps',page:'adv_port_trigger.asp'},		{title:'_gaming',page:'adv_port_range.asp'},
			{title:'_alg',page:'adv_alg.asp'},						{title:'_dos',page:'adv_dos.asp'},
			{title:'ipv6_firewall',page:'adv_ipv6_firewall.asp'}
		],
		//usb(6)
		[
			{title:'_samba_server',page:'smbserver.asp'},			{title:'_ftp_server',page:'ftpserver.asp'},
			{title:'_bt_service',page:'bt_settings.asp'},			{title:'_eject_device',page:'ejectdevice.asp'}
		],
		//help(7)
		[
			{title:'ish_menu',page:'help_menu.asp'},				{title:'_network',page:'help_network.asp'},
			{title:'_wireless',page:'help_wireless.asp'},			{title:'_advanced',page:'help_advanced.asp'},
			{title:'ADMIN',page:'help_administrator.asp'}
		]
	];

	
	this.build_structure = function(category, idx, sub_idx)
	{
		var content_main="";
		var which;
		var total;
		content_main += '<div class="arrowlistmenu">';
		content_main += '<div class="homenav" style="margin-bottom:20px;">';
		if(category=='0'){
			which = basic_L1;
			total = which.length;
			content_main += '<a href="/basic_status.asp"><span class="category_1" id="category_basic">'+get_words('_basic')+'</span></a>';
			content_main += '<a href="/adm_status.asp"><span class="category_0" id="category_advanced">'+get_words('_advanced')+'</span></a>';
		}else{
			which = advanced_L1;
			//total = (this.usb?which.length:--which.length);
			total = which.length;
			content_main += '<a href="/basic_status.asp"><span class="category_0" id="category_basic">'+get_words('_basic')+'</span></a>';
			content_main += '<a href="/adm_status.asp"><span class="category_1" id="category_advanced">'+get_words('_advanced')+'</span></a>';
		}
		content_main += '</div>';
		content_main += '<div class="borderbottom"> </div>';
		for(var i=0; i<total; i++)
		{
			var img_str = '', class_str = '', click_str = '', title_str = '';
			var title_lang;

			if (which[i].img)
				img_str = which[i].img;

			if (which[i].page)
				click_str = 'onclick="menuObject.animoa(this,\''+ which[i].page +'\');"';
			else
				click_str = 'onclick="menuObject.animoa(this);"';

			if (i == idx) {
				class_str = 'class="menuheader expandable openheader"';
				if (img_str)
					img_str = '<img src="/image/'+which[i].img+'_1.png" class="CatImage" />';
			} else {
				class_str = 'class="menuheader expandable closeheader"';
				if (img_str)
					img_str = '<img src="/image/'+which[i].img+'_0.png" class="CatImage" />';
			}

			title_lang = get_words(which[i].title);
			if (i == 0) {
				title_lang += ' ' + changes_count;
				if (changes_count > 0) {
					if (img_str)
						title_str = '<span class="unsaved_blink">'+ title_lang +'</span></div>';
					else
						title_str = '<span class="unsaved_blink" style="font: bold 16px Arial;">'+ title_lang +'</span></div>';
				} else {
					if (img_str)
						title_str = '<span class="CatTitle">'+ title_lang +'</span></div>';
					else
						title_str = '<span class="CatTitle" style="font: bold 16px Arial;">'+ title_lang +'</span></div>';
				}
			} else {
				if (which[i].style)
					title_str = '<span class="CatTitle" style="' + which[i].style +'">'+ title_lang +'</span></div>';
				else
					title_str = '<span class="CatTitle">'+ title_lang +'</span></div>';
			}
			content_main += '<div><div ' + click_str + ' ' + class_str + '>' +
					img_str + title_str;
			content_main += this.build_sub_structure(category, i, sub_idx, (i == idx));
			content_main += '</div>';
		}
		content_main += '</div>';
		//$("#main_title").html(content_main);
		return content_main;
	};

	this.build_sub_structure = function(category, idx, sub_idx, expand)
	{
		var which = new Array();
		var content_sub='';
		if(category==1)
		{
			which = advanced_L2[idx];
		}
		if(expand)
			content_sub += '<ul class="categoryitems">';
		else
			content_sub += '<ul class="categoryitems" style="display:none;">';
		for(var j=0; j<which.length; j++)
		{
			if(j==sub_idx && expand)
				content_sub += '<li><a href="'+ which[j].page +'" style="color:#00aff0;">'+ get_words(which[j].title) +'</a></li>';
			else
				content_sub += '<li><a href="'+ which[j].page +'">'+ get_words(which[j].title) +'</a></li>';
		}
		content_sub += '</ul>';
		return content_sub;
	};
	
	this.setSupportUSB = function(is){
		this.usb = is;
	};
}

var $j = $;

menuObject.animoa = function(node, redirect){
//	console.log(redirect);
	var src = $j('.menuheader.expandable.openheader').find('img').attr('src');
	if(src != undefined)
		$j('.menuheader.expandable.openheader').find('img').attr('src', src.replace('_1.','_0.'));
	$j('.menuheader.expandable.openheader').toggleClass('openheader').toggleClass('closeheader');
	$j(node).toggleClass('closeheader').toggleClass('openheader');
	src = $j(node).find('img').attr('src');
	if(src != undefined)
		$j(node).find('img').attr('src', src.replace('_0.','_1.'));
	$j('.categoryitems').slideUp();
	$j(node).parent().children('ul').slideDown(400, function(){
		if(redirect!=undefined)
			location.assign(redirect);
	});
};

function write_sb_menu_btn() {
	var str_bandwidth = '<input name="btn_bandwidth" type="button" class="ButtonSmall" id="btn_bandwidth" ' +
		'style="width:130px;margin-top:15pt;" onClick="window.location.href=\'/basic_sb_bandwidth.asp\'" ' +
		'value="' + get_words('_bandwidth') + '">';

	var str_mynetwork = '<input name="btn_mynetwork" type="button" class="ButtonSmall" id="btn_mynetwork" ' +
		'style="width:135px;margin-top:15pt;" onClick="window.location.href=\'/basic_sb_fxnetwork.asp\'" ' +
		'value="' + get_words('_mynetwork') + '">';

	var str_priorities = '<input name="btn_priorities" type="button" class="ButtonSmall" id="btn_priorities" ' +
		'style="width:130px;margin-top:15pt;" onClick="window.location.href=\'/basic_sb_fxpriority.asp\'" ' +
		'value="' + get_words('_priorities') + '">';

	var str_up_time = '<input name="btn_stat_up_time" type="button" class="ButtonSmall" id="btn_stat_up_time" ' +
		'style="width:150px;margin-top:15pt;" onClick="window.location.href=\'/basic_sb_fxtopbytime.asp\'" ' +
		'value="' + get_words('_stat_up_time') + '">';

	var str_downloads = '<input name="btn_stat_downloads" type="button" class="ButtonSmall" id="btn_stat_downloads" ' +
		'style="width:150px;margin-top:15pt;" onClick="window.location.href=\'/basic_sb_fxtopbydl.asp\'" ' +
		'value="' + get_words('_stat_downloads') + '">';

	document.write(str_bandwidth+str_mynetwork+str_priorities+str_up_time+str_downloads);
}

