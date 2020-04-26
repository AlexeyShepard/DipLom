<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class PeopleContact extends Model
{
  /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_PeopleContact';

  /**
    * @var bool
    */
    public $timestamps = false;
}
