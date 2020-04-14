using Godot;
using System;
using Godotcraft.scripts;
using Console = Godotcraft.scripts.objects.Console;

// taken from https://github.com/turtlewit/VineCrawler/blob/master/PlayerNew.gd
// flying by minidigger
public class Player : KinematicBody {
	[Export] public float mouseSensitivity = .1f;

	[Export] public float gravity = 20f;

	[Export] public float friction = 6.0f;

	[Export] public float moveSpeed = 15.0f;
	[Export] public float runAcceleration = 14.0f;
	[Export] public float runDeacceleration = 10.0f;
	[Export] public float airAcceleration = 2.0f;
	[Export] public float airDeacceleration = 2.0f;
	[Export] public float airControlVal = 0.3f;
	[Export] public float sideStrafeAcceleration = 50.0f;
	[Export] public float sideStrafeSpeed = 1.0f;
	[Export] public float jumpSpeed = 8.0f;
	[Export] public float moveScale = 1.0f;

	[Export] public float groundSnapTolerance = 1f;

	private Vector3 moveDirectionNorm;
	private Vector3 playerVelocity;

	private bool wishJump;
	private bool flying;

	private bool touchingGround;

	private float forwardMove;
	private float rightMove;

	private Camera camera;

	public override void _Ready() {
		Input.SetMouseMode(Input.MouseMode.Captured);
		SetPhysicsProcess(true);

		camera = GetNode<Camera>("Camera");
	}

	public override void _PhysicsProcess(float delta) {
		queueJump();
		if (touchingGround || flying) {
			groundMove(delta);
		}
		else {
			airMove(delta);
		}

		if (flying) {
			flyControls(delta);
		}

		playerVelocity = MoveAndSlide(playerVelocity, Vector3.Up);
		touchingGround = IsOnFloor();

		if (touchingGround && flying) {
			flying = false;
		}
	}

	private void snap_to_ground(Vector3 from) {
		var to = from + -GlobalTransform.basis.y * groundSnapTolerance;
		var spaceState = GetWorld().DirectSpaceState;

		var result = spaceState.IntersectRay(from, to);
		if (result.Count != 0) {
			var transform = GlobalTransform;
			transform.origin.y = ((Vector3) result["position"]).y;
			GlobalTransform = transform;
		}
	}

	private void flyControls(float delta) {
		if (Input.IsActionPressed(Actions.movement_jump)) {
			playerVelocity.y += jumpSpeed;
		}

		if (Input.IsActionPressed(Actions.movement_sneak)) {
			playerVelocity.y -= jumpSpeed;
		}
	}

	private void setMovementDir() {
		forwardMove = 0;
		rightMove = 0;
		if (!Console.instance.isConsoleShown) {
			forwardMove += Convert.ToInt32(Input.IsActionPressed(Actions.movement_walk_forwards));
			forwardMove -= Convert.ToInt32(Input.IsActionPressed(Actions.movement_walk_backwards));
			rightMove += Convert.ToInt32(Input.IsActionPressed(Actions.movement_strafe_right));
			rightMove -= Convert.ToInt32(Input.IsActionPressed(Actions.movement_strafe_left));
		}
	}

	private void queueJump() {
		if (Input.IsActionJustPressed(Actions.movement_jump) && !Console.instance.isConsoleShown) {
			wishJump = true;
		}

		if (Input.IsActionJustReleased(Actions.movement_jump)) {
			wishJump = false;
		}
	}

	private void airMove(float delta) {
		var wishDir = new Vector3();
		var wishVel = airAcceleration;
		float accel;

		var scale = cmdScale();

		setMovementDir();

		wishDir += Transform.basis.x * rightMove;
		wishDir -= Transform.basis.z * forwardMove;

		var wishSpeed = wishDir.Length();
		wishSpeed *= moveSpeed;

		wishDir = wishDir.Normalized();
		moveDirectionNorm = wishDir;

		var wishSpeed2 = wishSpeed;
		accel = playerVelocity.Dot(wishDir) < 0 ? airDeacceleration : airAcceleration;

		if ((forwardMove == 0) && (rightMove != 0)) {
			if (wishSpeed > sideStrafeSpeed) {
				wishSpeed = sideStrafeSpeed;
			}

			accel = sideStrafeAcceleration;
		}

		accelerate(wishDir, wishSpeed, accel, delta);
		if (airControlVal > 0) {
			airControl(wishDir, wishSpeed2, delta);
		}

		if (wishJump) {
			flying = true;
		}
		else {
			playerVelocity.y -= gravity * delta;
		}
	}

	private void airControl(Vector3 wishDir, float wishSpeed, float delta) {
		if ((Mathf.Abs(forwardMove) < 0.001) || (Mathf.Abs(wishSpeed) < 0.001)) {
			return;
		}

		var zSpeed = playerVelocity.y;
		playerVelocity.y = 0;

		var speed = playerVelocity.Length();
		playerVelocity = playerVelocity.Normalized();

		var dot = playerVelocity.Dot(wishDir);
		float k = 32.0f;
		k *= airControlVal * dot * dot * delta;

		if (dot > 0) {
			playerVelocity.x = playerVelocity.x * speed + wishDir.x * k;
			playerVelocity.y = playerVelocity.y * speed + wishDir.y * k;
			playerVelocity.z = playerVelocity.z * speed + wishDir.z * k;

			playerVelocity = playerVelocity.Normalized();
			moveDirectionNorm = playerVelocity;
		}

		playerVelocity.x *= speed;
		playerVelocity.y = zSpeed;
		playerVelocity.z *= speed;
	}

	private void groundMove(float delta) {
		var wishDir = new Vector3();

		if (!wishJump) {
			applyFriction(1.0f, delta);
		}
		else {
			applyFriction(0, delta);
		}

		setMovementDir();

		var scale = cmdScale();

		wishDir += Transform.basis.x * rightMove;
		wishDir -= Transform.basis.z * forwardMove;

		wishDir = wishDir.Normalized();
		moveDirectionNorm = wishDir;

		var wishSpeed = wishDir.Length();
		wishSpeed *= moveSpeed;

		accelerate(wishDir, wishSpeed, runAcceleration, delta);

		playerVelocity.y = 0.0f;

		if (wishJump) {
			playerVelocity.y = jumpSpeed;
			wishJump = false;
		}
	}

	private void applyFriction(float t, float delta) {
		var vec = playerVelocity;

		vec.y = 0.0f;
		float speed = vec.Length();
		var drop = 0.0f;

		if (touchingGround || flying) {
			var control = speed < runDeacceleration ? runDeacceleration : speed;
			drop = control * friction * delta * t;
		}

		var newSpeed = speed - drop;
		if (newSpeed < 0) {
			newSpeed = 0;
		}

		if (speed > 0) {
			newSpeed /= speed;
		}

		playerVelocity.x *= newSpeed;
		playerVelocity.z *= newSpeed;
	}

	private void accelerate(Vector3 wishDir, float wishSpeed, float accel, float delta) {
		var currentSpeed = playerVelocity.Dot(wishDir);
		var addSpeed = wishSpeed - currentSpeed;
		if (addSpeed <= 0)
			return;
		var accelSpeed = accel * delta * wishSpeed;
		if (accelSpeed > addSpeed)
			accelSpeed = addSpeed;

		playerVelocity.x += accelSpeed * wishDir.x;
		playerVelocity.z += accelSpeed * wishDir.z;
	}

	private float cmdScale() {
		var varMax = (int) Mathf.Abs(forwardMove);
		if (Mathf.Abs(rightMove) > varMax) {
			varMax = (int) Mathf.Abs(rightMove);
		}

		if (varMax <= 0) {
			return 0;
		}

		var total = Mathf.Sqrt(forwardMove * forwardMove + rightMove * rightMove);
		var scale = moveSpeed * varMax / (moveScale * total);

		return scale;
	}

	public override void _Input(InputEvent @event) {
		if (@event is InputEventMouseMotion motion && !Console.instance.isConsoleShown) {
			RotateY(-Mathf.Deg2Rad(motion.Relative.x) * mouseSensitivity);
			camera.RotateX(-Mathf.Deg2Rad(motion.Relative.y) * mouseSensitivity);
		}
	}
}