﻿
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  
  <title>Animating with Modernizr &middot; jQuery Masonry</title>
  
  <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
  <!--[if lt IE 9]><script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script><![endif]-->
  
    <link href="Views/../Content/masonryStyle.css" rel="stylesheet" type="text/css" />
    <script src="Views/../Scripts/Masonry/modernizr-transitions.js" type="text/javascript"></script>
    <script src="Views/../Scripts/Masonry/jquery-1.6.2.min.js"></script>
    <script src="Views/../Scripts/Masonry/jquery.masonry.min.js"></script>
    <script>
        $(function () {

            $('#container').masonry({
                itemSelector: '.box',
                columnWidth: 100,
                isAnimated: !Modernizr.csstransitions
            });

        });
    </script>
  <!-- scripts at bottom of page -->

</head>
<body class="demos ">
  
  <section id="content">
    
    
      <h1>Animating with Modernizr</h1>
    
    
    <div id="copy">
  <p>This layout is animated with CSS transitions if the browser supports it, but falls back to jQuery animation if not available. Modernizr has been implemented to detect support. More details in <a href="Views/docs/animating.html#modernizr">Animating</a>.</p></p>
</div>

<div id="container" class="transitions-enabled clearfix">

  <div class="box col2">
    Sit amet mi ullamcorper vehicula
  </div>

  <div class="box col1">
    <p>Phasellus pede arcu, dapibus eu, fermentum et, dapibus sed, urna.</p>
  </div>

  <div class="box col2">
    Ut condimentum mi vel tellus. Suspendisse laoreet. Fusce ut est sed dolor gravida convallis. Morbi vitae ante. Vivamus ultrices luctus nunc. Suspendisse et dolor. Etiam dignissim. Proin malesuada adipiscing lacus. Donec metus. Curabitur gravida.
    <p>Phasellus pede arcu, dapibus eu, fermentum et, dapibus sed, urna.</p>
  </div>

  <div class="box col3">
    <p>Sed ac risus. Phasellus lacinia, magna a ullamcorper laoreet, lectus arcu pulvinar risus, vitae facilisis libero dolor a purus. Sed vel lacus. Mauris nibh felis, adipiscing varius, adipiscing in, lacinia vel, tellus. Suspendisse ac urna. Etiam pellentesque mauris ut lectus. Nunc tellus ante, mattis eget, gravida vitae, ultricies ac, leo. Integer leo pede, ornare a, lacinia eu, vulputate vel, nisl.</p>
  </div>

  <div class="box col1">
    <p>Morbi purus libero, faucibus adipiscing, commodo quis, gravida id, est. Sed lectus. Praesent elementum hendrerit tortor. Sed semper lorem at felis.</p>
  </div>

  <div class="box col2">
    <p>Vestibulum volutpat, lacus a ultrices sagittis,</p>
    <p>Fusce accumsan mollis eros. Pellentesque a diam sit amet mi ullamcorper vehicula  </p>
  </div>

  <div class="box col2">
    <p>Sit amet mi ullamcorper vehicula</p>
  </div>

  <div class="box col1">
    <p>Morbi purus libero</p>
  </div>

  <div class="box col1">
    <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec odio. Quisque volutpat mattis eros. Nullam malesuada erat ut turpis. Suspendisse urna nibh, viverra non, semper suscipit, posuere a, pede.</p>
  </div>

  <div class="box col3">
    <p>Donec nec justo eget felis facilisis fermentum. Aliquam porttitor mauris sit amet orci. Aenean dignissim pellentesque felis.</p>
    <p>Vestibulum volutpat, lacus a ultrices sagittis,</p>
    <ul>
      <li>Lacus a ultrices sagittis</li>
      <li>Democratis</li>
      <li>Plummus</li>
    </ul>
  </div>

  <div class="box col1">
    <p>Fusce accumsan mollis eros. Pellentesque a diam sit amet mi ullamcorper vehicula  </p>
  </div>

  <div class="box col1">
    <p>Sit amet mi ullamcorper vehicula</p>
  </div>

  <div class="box col1">
    <h2>Morbi purus libero</h2>
  </div>


  <div class="box col2">
    <p>Ut condimentum mi vel tellus. Suspendisse laoreet. Fusce ut est sed dolor gravida convallis. Morbi vitae ante. Vivamus ultrices luctus nunc. Suspendisse et dolor. Etiam dignissim. Proin malesuada adipiscing lacus. Donec metus. Curabitur gravida.</p>
    <p>Phasellus pede arcu, dapibus eu, fermentum et, dapibus sed, urna.</p>
  </div>

  <div class="box col1">
    <p>Fusce accumsan mollis eros. Pellentesque a diam sit amet mi ullamcorper vehicula</p>
  </div>




  <div class="box col1">
    <p>adipiscing in, lacinia vel, tellus. Suspendisse ac urna. Etiam pellentesque mauris ut lectus.</p>
  </div>

  <div class="box col2">
    <p>Sit amet mi ullamcorper vehicula</p>
  </div>

  <div class="box col1">
    <p>Phasellus pede arcu, dapibus eu, fermentum et, dapibus sed, urna.</p>
  </div>



  <div class="box col3">
    <p>Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Phasellus hendrerit. Pellentesque aliquet nibh nec urna. In nisi neque, aliquet vel, dapibus id, mattis vel, nisi. Sed pretium, ligula sollicitudin laoreet viverra, tortor libero sodales leo, eget blandit nunc tortor eu nibh. Nullam mollis. Ut justo. Suspendisse potenti.</p>
    <p>Fusce accumsan mollis eros. Pellentesque a diam sit amet mi ullamcorper vehicula  </p>
    <p>Morbi interdum mollis sapien. Sed ac risus. Phasellus lacinia, magna a ullamcorper laoreet, lectus arcu pulvinar risus, vitae facilisis libero dolor a purus.</p>
  </div>

  <div class="box col2">
    <p>adipiscing in, lacinia vel, tellus. Suspendisse ac urna. Etiam pellentesque mauris ut lectus.</p>
    <p>Ut convallis, sem sit amet interdum consectetuer, odio augue aliquam leo, nec dapibus tortor nibh sed augue.  Sed vel lacus. Mauris nibh felis, adipiscing varius, adipiscing in, lacinia vel, tellus. Suspendisse ac urna. Etiam pellentesque mauris ut lectus. Nunc tellus ante, mattis eget, gravida vitae, ultricies ac, leo. Integer leo pede, ornare a, lacinia eu, vulputate vel, nisl.</p>
  </div>

  <div class="box col1">
    <p>Phasellus pede arcu, dapibus eu, fermentum et, dapibus sed, urna.</p>
  </div>

  <div class="box col1">
    <p>Fusce accumsan mollis eros. Pellentesque a diam sit amet mi ullamcorper vehicula</p>
  </div>

</div> <!-- #container -->


    
    <footer id="site-footer">
      jQuery Masonry by <a href="http://desandro.com">David DeSandro</a>
    </footer>
    
  </section> <!-- #content -->

</body>
</html>