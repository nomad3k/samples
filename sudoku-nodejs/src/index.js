import Puzzle from './puzzle';

let puzzle = new Puzzle();

puzzle.set([
  [ 0, 0, 0, 3, 4, 0, 8, 5, 0 ],
  [ 0, 0, 8, 0, 0, 0, 0, 0, 0 ],
  [ 5, 6, 9, 1, 8, 0, 0, 0, 4 ],
  [ 2, 0, 1, 0, 5, 4, 6, 0, 0 ],
  [ 0, 0, 0, 0, 0, 0, 0, 0, 0 ],
  [ 0, 0, 3, 7, 1, 0, 9, 0, 2 ],
  [ 7, 0, 0, 0, 9, 6, 4, 1, 5 ],
  [ 0, 0, 0, 0, 0, 0, 3, 0, 0 ],
  [ 0, 1, 5, 0, 3, 7, 0, 0, 0 ]
]);
puzzle.print();

if (!puzzle.validate(false)) {
  console.log('invalid');
} else {
  var start = new Date().getTime();
  let solution = puzzle.solve();
  var duration = new Date().getTime() - start;
  console.log('solution took %dms', duration);

  if (!solution) {
    console.log('No Solution!!!');
  } else {
    solution.print();
  }
}
