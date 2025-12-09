// app/Repositories/CmsRepository.php
<?php

namespace App\Repositories;

use App\Models\CmsBlock;
use Illuminate\Support\Facades\Cache;

class CmsRepository
{
    private const CACHE_TTL = 3600; // 1 час
    
    public function getBySlug(string $slug): ?CmsBlock
    {
        $cacheKey = "cms_block_{$slug}";
        
        return Cache::remember($cacheKey, self::CACHE_TTL, function () use ($slug) {
            return CmsBlock::where('slug', $slug)
                ->where('is_active', true)
                ->first();
        });
    }
    
    public function getContentBySlug(string $slug): string
    {
        $block = $this->getBySlug($slug);
        
        return $block ? $block->content : '';
    }
}