20 Dec 2008 - kitofr
I just realized that if we want to run the acceptance tests we need to first start the web-server 
(cassini) or register the site on an iis server. We also need to edit the config file to not persist
to file.

14 Jul 2008 - kitofr
Ok, so; I just found out (http://www.jameskovacs.com/blog/RunningWatiNTestsOnVista.aspx) that 
to actually run watin (the acceptance tests) using windows vista and IE 7, you need to 
turn off the [Protected Mode] in IE. To do this, you can either turn it totaly off, or
(as the article states) set http://localhost as a trusted site. 
I've done this for now, on my computer. Still want to have that (or at least a check for it)
in the tests. See if I can find a way to do that.
