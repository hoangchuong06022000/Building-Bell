jQuery(document).ready(function () {
    //jQuery(".SelectUI").addSelectUI(/*{optional}*/)
    if ( jQuery().addSelectUI != undefined ) {
        jQuery(".SelectUI").addSelectUI({
            scrollbarWidth: 15 //default is 10
        });
    }

    if ( jQuery().jcarousel != undefined ) {
        jQuery("#transList").jcarousel({
            offset: 1,
            scroll: 1,
            initCallback: transList_init_callback,
            onButtonAfterAnimationCallBack: animation_callback
        });
    }

    //Trigger show tooltip box when hover table row
    jQuery("table.AddressBook > tbody > tr").hover(
        function (e) { //on hover in
            jQuery("#addressBookTooltip")
                .removeClass("Hidden")
                .position({
                    my: 'left bottom',
                    of: jQuery(this),
                    offset: '150px 0',
                    collision: 'flip'
                });
        },
        function (e) { //on hover out
            jQuery("#addressBookTooltip").removeAttr("style").addClass("Hidden")
        }
    );

    //Fake password text on login form
    jQuery("#loginPwd").bind("focus", function (e) {
        jQuery("#fakeLoginPwdText").hide();
    });
    jQuery("#loginPwd").bind("blur", function (e) {
        if ( jQuery(this).val() == '' ) {
            jQuery("#fakeLoginPwdText").show();
        }
    });
});

/* transList_init_callback
 * use for carousel in 'residence_transsearch_2'
 */
function transList_init_callback (carousel) { //
    carousel.list.find("> li > a").bind("click", function (e) { e.preventDefault(); });

    //binding event to all items
    carousel.list.find("> li.jcarousel-item").each(function (index) {
        var item = jQuery(this);

        item.bind("click", function () {
            set_active(carousel, item);
            move_to_visible(carousel, item);

            //show content
            //custom function to reload data on showcase block
            reload_showcase_data(carousel);
        });

        item.bind("mouseover", function () { //hover
            carousel.list.find("> li.Hover").removeClass("Hover");
            item.addClass("Hover");
            return false;
        });
    });

    jQuery(document).bind("mouseover", function () { //hover remove
        carousel.list.find("> li.Hover").removeClass("Hover");
    });

    //active item
    var active_item = carousel.list.find("> li.Active");
    var list_array = carousel.list.find("> li");
    var active_index = jQuery.inArray(active_item.get(0), list_array);

    //set active on carousel
    set_active(carousel, active_item);

    //scroll carousel
    setTimeout(function () {
        carousel.scroll(active_index+1, /*animating*/false, /*callback*/function () {
            move_to_visible(carousel, active_item);
        }); //scroll to item
    }, 1);

    if (active_index == (list_array.length - 1)) {
      setTimeout(function() {
          carousel.next();
        }, 1);
    }

    //show content
    //custom function to reload data on showcase block
    reload_showcase_data(carousel);
}

/* set_active
 * use for carousel in 'residence_transsearch_2'
 * internally called, SHOULD NOT USE AS PUBLIC
 */
function set_active (carousel, active_item) {
    carousel.list.find("> li.Active").removeClass("Active"); //clear
    active_item.addClass("Active"); //set active
}

/* move_to_visible
 * use for carousel in 'residence_transsearch_2'
 * internally called, SHOULD NOT USE AS PUBLIC
 */
function move_to_visible (carousel, active_item) {
    if ( (offset = active_item.position().left + active_item.width() + carousel.list.position().left - carousel.clip.width()) > 0 ) {
        //force visible for active item (right-item)
        carousel.next();
    }
    else if ( active_item.position().left + carousel.list.position().left < 0 ) {
        //force visible for active item (left-item)
        carousel.prev();
    }
}

/* animation_callback
 * use for carousel in 'residence_transsearch_2'
 * internally called, SHOULD NOT USE AS PUBLIC
 */
function animation_callback (carousel) {
    var active_item = carousel.list.find("> li.Active");
    var active_index = jQuery.inArray(active_item.get(0), carousel.list.find("> li"));

    if ( active_item.length > 0 ) {
        if ( active_item.position().left + active_item.width() + carousel.list.position().left > carousel.clip.width() ) {
            //right-item-invisible
            //set active for prev item
            active_item.removeClass("Active");
            active_item.prev().addClass("Active");
            active_index--;
        }
        else if ( active_item.position().left + carousel.list.position().left < 0 ) { //left-item-invisible
            //set active for next item
            active_item.removeClass("Active");
            active_item.next().addClass("Active");
            active_index++;
        }
        //show content
        //custom function to reload data on showcase block
        reload_showcase_data(carousel);
    }
}

/* reload_showcase_data
 * use for carousel in 'residence_transsearch_2'
 * reload content when change active item in the carousel
 */
function reload_showcase_data (carousel) {
    //reload data on showcase and details
    //.BlockShowcase and .BlockDetailsInfo
}
// Zoom img in transaction detail
$(function() {
	$(".ShowCase").click(function() {
		$(".ShowcaseImg").attr("src", $(this).attr("rel"));
		 $(".ShowcaseImg").parent().attr("href", $(this).attr("href"));
	});
	if(jQuery(".ShowCaseZoom").length) {
		jQuery(".ShowCaseZoom").colorbox({
			onOpen: onShowCaseZoomOpen,
			onClosed: onShowCaseZoomClosed
		});
	}
});
function onShowCaseZoomOpen() {
	jQuery("#colorbox").addClass("ShowCaseColorbox");
}
function onShowCaseZoomClosed() {
	jQuery("#colorbox").removeClass("ShowCaseColorbox");
}
			
/*-----------------auto complete-------------*/
(function( $ ) {
		$.widget( "ui.combobox", {
			_create: function() {
				var self = this,
					select = this.element.hide(),
					selected = select.find( ":selected" ),
					value = selected.val() ? selected.text() : "";
				var opt = select.find('option:first');
				var inputstr = opt.val() == '-1' ? '<input type="textbox" rel="' + select.find('option:first').text() + '"/>' : '<input type="textbox"/>'
				var input = this.input = $(inputstr)
					.insertAfter( select )
					.val( value )
					.autocomplete({
						delay: 0,
						minLength: 0,
						source: function( request, response ) {
							var matcher = new RegExp( $.ui.autocomplete.escapeRegex(request.term), "i" );
							response( 
								select.find( "option" ).map(function() {
									var text = $( this ).text();
									if($(this).val() == -1) { return; }
									if ( this.value && ( !request.term || matcher.test(text) ) )
										return {
											label: ($(this).attr('child') == '1' ? "&nbsp;&nbsp;" : "") + 
												text.replace(
													new RegExp(
														"(?![^&;]+;)(?!<[^<>]*)(" +
														$.ui.autocomplete.escapeRegex(request.term) +
														")(?![^<>]*>)(?![^&;]+;)", "gi"
													), "<strong>$1</strong>"),
											value: text,
											isGrouped: $( this ).parent()[0].tagName.toLowerCase() == 'optgroup' ? true : false,
											option: this
										};
								})
							);
						},
						/* Pyco */
						open: function(event, ui) {
							$(".ui-autocomplete").jScrollPane();
						},
						close: function(event, ui) {},
						select: function( event, ui ) {
							ui.item.option.selected = true;
							self._trigger( "selected", event, {
								item: ui.item.option
							});
							// change event here
							selected.change(); 
							// change event
						},
						change: function( event, ui ) {
							if ( !ui.item ) {
								var matcher = new RegExp( "^" + $.ui.autocomplete.escapeRegex( $(this).val() ) + "$", "i" ),
									valid = false;
								select.find( "option" ).each(function() {
									if ( $( this ).text().match( matcher ) ) {
										this.selected = valid = true;
										return false;
									}
								});
								if ( !valid ) {
									// remove invalid value, as it didn't match anything
									$( this ).val( "" );
									select.val( "" );
									input.data( "autocomplete" ).term = "";
									return false;
								}
							}
						}
					})
					.addClass( "ui-widget ui-widget-content ui-corner-left" );
				input.data( "autocomplete" )._renderItem = function( ul, item ) {
					var liStr = '<li></li>';
					if(item.isGrouped) {
						liStr = '<li class="OptionGrouped"></li>';
						if($(item.option).index() == 0){
							var optStr = '<li><span>' + $(item.option).parent().attr("label") + '</span></li>';
							$(optStr).appendTo( ul );
						}
					}
					return $( liStr )
						.data( "item.autocomplete", item )
						.append( "<a>" + item.label + "</a>" ).appendTo( ul );
				};

				this.button = $( "<input type='button' />" )
					.attr( "tabIndex", -1 )
					.attr( "title", "Show All Items" )
					.insertAfter( input )
					.button({
						icons: {
							primary: "ui-icon-triangle-1-s"
						},
						text: false
					})
					.removeClass( "ui-corner-all" )
					.addClass( "ui-corner-right ui-button-icon" )
					.click(function() {
						// close if already visible
						if ( input.autocomplete( "widget" ).is( ":visible" ) ) {
							input.autocomplete( "close" );
							return;
						}

						// pass empty string as value to search for, displaying all results
						input.autocomplete( "search", "" );
						input.autocomplete( "open", "" ); /* Pyco */
						input.focus();						
					});
			},

			destroy: function() {
				this.input.remove();
				this.button.remove();
				this.element.show();
				$.Widget.prototype.destroy.call( this );
			}
		});
	})( jQuery );

	$(function() {
		$( ".combobox" ).combobox();
		$( "#toggle" ).click(function() {
			$( ".combobox" ).toggle();
		});
	});		
/*-----------------auto complete-------------*/