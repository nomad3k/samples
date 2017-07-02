import Solution from './solution';

export default class Puzzle {
  constructor() {
    this.cells = [];
    for (let r=0;r<9;r++) {
      this.cells[r] = [];
      for (let c=0;c<9;c++) {
        this.cells[r][c] = 0;
      }
    }
  }

  clone() {
    return new Puzzle().set(this.cells);
  }

  setValue(r, c, v) {
    if (r < 0 || r >= 9) throw new Error('Invalid Parameter: r');
    if (c < 0 || c >= 9) throw new Error('Invalid Parameter: c');
    if (c < 0 || c > 9) throw new Error('Invalid Parameter: v');
    this.cells[r][c] = v;
  }

  set(v) {
    for (let r=0;r<9;r++) {
      for (let c=0;c<9;c++) {
        this.cells[r][c] = v[r][c];
      }
    }
    return this;
  }

  validate(complete = true) {
    // unique rows
    for (let r=0;r<9;r++) {
      let values = [];
      for (let c=0;c<9;c++) {
        let cell = this.cells[r][c];
        if (complete && cell == 0 || cell != 0 && values.indexOf(cell) != -1) {
          console.log('problem', r,c);
          return false;
        }
        values.push(cell);
      }
    }
    // unique columns
    for (let c=0;c<9;c++) {
      let values = [];
      for (let r=0;r<9;r++) {
        let cell = this.cells[r][c];
        if (complete && cell == 0 || cell != 0 && values.indexOf(cell) != -1)
          return false;
        values.push(cell)
      }
    }
    // grids
    for (let o=0;o<9;o++) {
      let values = [];
      for (let i=0;i<9;i++) {
        let r = Math.floor(o / 3) * 3 + Math.floor(i / 3);
        let c = o % 3 * 3 + i % 3;
        let cell = this.cells[r][c];
        if (complete && cell == 0 || cell != 0 && values.indexOf(cell) != -1)
          return false;
        values.push(cell);
      }
    }
    return true;
  }

  print() {
    console.log('+-------------+');
    for (let r=0;r<9;r++) {
      if (r % 3 == 0)
        console.log('|             |');
      let row = '|'
      for (let c=0;c<9;c++) {
        if (c % 3 == 0)
          row += ' ';
        row += this.cells[r][c] || '.';
      }
      row += ' |';
      console.log(row);
    }
    console.log('|             |');
    console.log('+-------------+')
  }

  solve() {
    return new Solution(this, 0, 0).execute();
  }
}
