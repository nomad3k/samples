export default class Solution {

  constructor(puzzle, row, col) {
    this.puzzle = puzzle.clone();
    this.row = row;
    this.col = col;
    this.values = this.possibleValues();
  }

  possibleValues() {
    if (this.puzzle.cells[this.row][this.col] > 0) {
      return [
        this.puzzle.cells[this.row][this.col]
      ];
    } else {
      let values = [ 1, 2, 3, 4, 5, 6, 7, 8, 9 ];
      this.exclude(this.puzzle.cells[this.row][this.col], values);
      for (let c=0;c<9;c++) {
        this.exclude(this.puzzle.cells[this.row][c], values);
      }
      for (let r=0;r<9;r++) {
        this.exclude(this.puzzle.cells[r][this.col], values);
      }
      let rr = Math.floor(this.row / 3) * 3;
      let cc = Math.floor(this.col / 3) * 3;
      for (let r=rr;r<rr+3;r++) {
        for (let c=cc;c<cc+3;c++) {
          this.exclude(this.puzzle.cells[r][c], values);
        }
      }
      let x = [];
      while (values.length > 0) {
        let ix = Math.floor(Math.random() * values.length);
        x.push(values[ix]);
        values.splice(ix, 1);
      }
      return x;
    }
  }

  exclude(v, values) {
    if (v == 0) return;
    let ix = values.indexOf(v);
    if (ix >= 0) values.splice(ix, 1);
  }

  execute() {
    // no possibilities
    if (this.values.length == 0) return null;
    for (let v=0;v<this.values.length;v++) {
      this.puzzle.setValue(this.row, this.col, this.values[v]);
      if (this.row == 8 && this.col == 8) {
        return this.puzzle.validate() ? this.puzzle : null;
      } else {
        let next = new Solution(this.puzzle,
                                this.col == 8 ? (this.row + 1) : this.row,
                                (this.col + 1) % 9);
        let result = next.execute();
        if (result) return result;
      }
    }
    // run out of options
    return null;
  }

}
