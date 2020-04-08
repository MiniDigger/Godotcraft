extends Spatial

# The percentage of the screen that will move the camera when the mouse is over it.
const SCREEN_AMOUNT = 0.1;
# The speed the camera will rotate at.
const ROTATION_SPEED = 2;

func _ready():
	pass

func _process(delta):
	# Get the width of the screen.
	var screen_width = get_viewport().size.x;
	# Figure out how many pixels we need to look for, and assign the result to amount_to_look_for.
	var amount_to_look_for = screen_width * SCREEN_AMOUNT;
	# Get the position of the mouse.
	var mouse_position = get_viewport().get_mouse_position();
	
	# If the mouse is less than amount_to_look_for, then the mouse most be over
	# the percentage of the screen that will move the camera on the left side.
	if (mouse_position.x < amount_to_look_for):
		# Rotate the camera.
		rotate_y(-ROTATION_SPEED * delta);
	
	# If the mouse is more than screen_width minus amount_to_look_for, then the mouse
	# most be over the percentage of the screen that will move the camera on the right side.
	elif (mouse_position.x > screen_width - amount_to_look_for):
		# Rotate the camera.
		rotate_y(ROTATION_SPEED * delta);
