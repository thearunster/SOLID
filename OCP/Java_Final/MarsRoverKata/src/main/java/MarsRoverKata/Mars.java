package MarsRoverKata;

import com.google.common.base.Predicate;
import com.google.common.collect.Collections2;
import com.google.common.collect.Iterables;

import java.util.ArrayList;
import java.util.List;

public class Mars {
    private Size bounds; // get
    private Point centerOfThePlanet; // get

    private final List<Obstacle> obstacles;

    public List<Obstacle> getObstacles() {
        return obstacles;
    }

    public Mars() {
        this(new Size(25, 25));
    }

    public Mars(Size bounds) {
        this.bounds = bounds;
        centerOfThePlanet = new Point(this.bounds.getWidth() / 2, this.bounds.getHeight() / 2);
        obstacles = new ArrayList<Obstacle>();
    }

    public Size getBounds() {
        return bounds;
    }

    public Point getCenterOfThePlanet() {
        return centerOfThePlanet;
    }

    public void addObstacle(Obstacle obstacle) {
        obstacles.add(obstacle);
    }

    public void removeObstacle(Obstacle obstacle) {
        obstacles.remove(obstacle);
    }

    public Point calculateFinalPosition(Point from, Point desired) {
        Point newDestination = desired;
        newDestination = calculatePositionY(desired, newDestination);
        newDestination = calculatePositionX(desired, newDestination);

        if (!isValidPosition(newDestination)) {
            return from;
        }

        return newDestination;
    }

    private Point calculatePositionX(Point desired, Point newDestination) {
        if (desired.getX() > bounds.getWidth()) {
            newDestination = new Point(0, desired.getY());
        }
        if (desired.getX() < 0) {
            newDestination = new Point(bounds.getWidth(), desired.getY());
        }
        return newDestination;
    }

    private Point calculatePositionY(Point desired, Point newDestination) {
        if (desired.getY() > bounds.getHeight()) {
            newDestination = new Point(desired.getX(), 0);
        }
        if (desired.getY() < 0) {
            newDestination = new Point(desired.getX(), bounds.getHeight());
        }
        return newDestination;
    }

    public boolean isValidPosition(final Point point) {
        boolean anyInstance = Iterables.any(obstacles, new Predicate<Obstacle>() {
            @Override
            public boolean apply(Obstacle input) {
                return input.getLocation().equals(point);
            }
        });
        return !anyInstance;
    }
}

