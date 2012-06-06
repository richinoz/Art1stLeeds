<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/strict.dtd">
<html>
<head>
	<meta http-equiv="Content-Type" content="text/html;charset=iso-8859-1"/>
	<title>Lava Lamp Menu Tutorial</title>
	<script type="text/javascript" src="Scripts/jquery-1.5.1.min.js"></script>
    <script src="Scripts/jquery.easing.1.3.js" type="text/javascript"></script>
	<script type="text/javascript">

	    $(document).ready(function () {

	        //transitions
	        //for more transition, goto http://gsgd.co.uk/sandbox/jquery/easing/
	        var style = 'easeOutElastic';

	        //Retrieve the selected item position and width
	        var default_left = Math.round($('#lava li.selected').offset().left - $('#lava').offset().left);
	        var default_width = $('#lava li.selected').width();

	        //Set the floating bar position and width
	        $('#box').css({ left: default_left });
	        $('#box .head').css({ width: default_width });

	        //if mouseover the menu item
	        $('#lava li').hover(function () {

	            //Get the position and width of the menu item
	            left = Math.round($(this).offset().left - $('#lava').offset().left);
	            width = $(this).width();

	            //Set the floating bar position, width and transition
	            $('#box').stop(false, true).animate({ left: left }, { duration: 1000, easing: style });
	            $('#box .head').stop(false, true).animate({ width: width }, { duration: 1000, easing: style });

	            //if user click on the menu
	        }).click(function () {

	            //reset the selected item
	            $('#lava li').removeClass('selected');

	            //select the current item
	            $(this).addClass('selected');

	        });

	        //If the mouse leave the menu, reset the floating bar to the selected item
	        $('#lava').mouseleave(function () {

	            //Retrieve the selected item position and width
	            default_left = Math.round($('#lava li.selected').offset().left - $('#lava').offset().left);
	            default_width = $('#lava li.selected').width();

	            //Set the floating bar position, width and transition
	            $('#box').stop(false, true).animate({ left: default_left }, { duration: 1500, easing: style });
	            $('#box .head').stop(false, true).animate({ width: default_width }, { duration: 1500, easing: style });

	        });

	    });
	

	</script>
	
	<style type="text/css">

	body {
		font-family:georgia; 
		font-size:14px; 
	}
	a {
		text-decoration:none; 
		color:#333;
	}
	
	#lava {
		/* you must set it to relative, so that you can use absolute position for children elements */
		position:relative; 
		text-align:center; 
		width:783px; 
		height:40px;
		float:right;
	}
	
	#lava ul {
		/* remove the list style and spaces*/
		margin:0; 
		padding:0; 
		list-style:none; 
		display:inline;
				
		/* position absolute so that z-index can be defined */
		position:absolute; 
		
		/* center the menu, depend on the width of you menu*/
		left:110px; 
		top:0; 
		
		/* should be higher than #box */
		z-index:100;

	}
	
	#lava ul li {
		
		/* give some spaces between the list items */
		margin:0 15px; 
		
		/* display the list item in single row */
		float:left;
	}
	
	#lava #box {
		
		/* position absolute so that z-index can be defined and able to move this item using javascript */
		position:absolute; 
		left:0; 
		top:0; 
		
		/* should be lower than the list menu */
		z-index:50; 

		/* image of the right rounded corner */
		background:#ccc; 
		height:20px;
		
		/* add padding 8px so that the tail would appear */
		padding-right:8px;
		
		/* self-adjust negative margin to make sure the box display in the center of the item */
		margin-left:-10px;
	}
	
	#lava #box .head {
		/* image of the left rounded corner */
		background:#eee;
		height:20px;

		/* self-adjust left padding to make sure the box display in the center of the item */
		padding-left:10px;
	}
	</style>
	
</head>
<body>

<div id="lava">

	<ul>
        <li><a href="scrollExample.aspx">scrollExample</a></li>
		<li><a href="#">home</a></li>
		<li><a href="#">lava lamp menu</a></li>
		<li><a href="#">queness.com</a></li>
		<li class="selected"><a href="#">jQuery</a></li>			
	</ul>

	<!-- If you want to make it even simpler, you can append these html using jquery -->
	<div id="box"><div class="head"></div></div>

</div>


</body>
</html>